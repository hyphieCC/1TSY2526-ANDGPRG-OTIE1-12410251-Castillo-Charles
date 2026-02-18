using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QTESkillCheck : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private Image zoneImage;
    [SerializeField] private RectTransform needle;
    [SerializeField] private Image resultFlash;

    [Header("Input")]
    [SerializeField] private KeyCode confirmKey = KeyCode.Space;

    [Header("Randomized Difficulty")]
    [Tooltip("Needle speed range (clock degrees/sec). Higher = harder.")]
    [SerializeField] private float minNeedleSpeedDegPerSec = 200f;
    [SerializeField] private float maxNeedleSpeedDegPerSec = 280f;

    [Tooltip("Zone size range (degrees). Smaller = harder.")]
    [SerializeField] private float minZoneDegrees = 25f;
    [SerializeField] private float maxZoneDegrees = 45f;

    [Header("Tuning")]
    [SerializeField] private float flashDuration = 0.2f;

    [Tooltip("Delay before the needle starts moving (so player can see the zone).")]
    [SerializeField] private float preSpinDelay = 0.35f;

    [Tooltip("Needle completes exactly one 360 cycle. If not pressed by end -> fail.")]
    [SerializeField] private bool oneCycleOnly = true;

    [Header("Zone Spawn Range (Clock Degrees)")]
    [Tooltip("0=12, 90=3, 180=6, 270=9 (clockwise).")]
    [SerializeField] private bool limitZoneRange = true;
    [SerializeField] private float zoneStartMinClockDeg = 180f;
    [SerializeField] private float zoneStartMaxClockDeg = 330f;

    [Header("Player References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody playerRb;

    private bool active;
    private bool spinning;
    private float preSpinTimer;

    private float needleClockDeg;
    private float needleSpeedDegPerSec;
    private float cycleRemainingDeg;

    private float zoneStartClockDeg;
    private float zoneEndClockDeg;
    private float zoneSizeDegrees;

    private float savedLinearDamping;
    private float savedAngularDamping;

    private System.Action onSuccess;
    private System.Action onFail;

    private void Awake()
    {
        if (root != null) root.SetActive(false);

        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (playerRb == null && playerMovement != null)
            playerRb = playerMovement.GetComponent<Rigidbody>();

        if (zoneImage != null)
        {
            zoneImage.type = Image.Type.Filled;
            zoneImage.fillMethod = Image.FillMethod.Radial360;
            zoneImage.fillOrigin = (int)Image.Origin360.Top;
            zoneImage.fillClockwise = true;
        }

        SetFlashAlpha(0f);
    }

    private void Update()
    {
        if (!active) return;

        if (!spinning)
        {
            preSpinTimer -= Time.deltaTime;
            if (preSpinTimer <= 0f)
                spinning = true;

            if (needle != null)
                needle.localRotation = Quaternion.identity;

            if (Input.GetKeyDown(confirmKey))
            {
                bool success = IsClockAngleInside(needleClockDeg, zoneStartClockDeg, zoneEndClockDeg);
                StartCoroutine(Resolve(success));
            }

            return;
        }

        float deltaDeg = needleSpeedDegPerSec * Time.deltaTime;

        needleClockDeg = (needleClockDeg + deltaDeg) % 360f;

        if (needle != null)
            needle.localRotation = Quaternion.Euler(0f, 0f, -needleClockDeg);

        if (oneCycleOnly)
        {
            cycleRemainingDeg -= deltaDeg;
            if (cycleRemainingDeg <= 0f)
            {
                StartCoroutine(Resolve(false));
                return;
            }
        }

        if (Input.GetKeyDown(confirmKey))
        {
            bool success = IsClockAngleInside(needleClockDeg, zoneStartClockDeg, zoneEndClockDeg);
            StartCoroutine(Resolve(success));
        }
    }

    public void StartSkillCheck(System.Action success, System.Action fail)
    {
        if (active) return;

        AudioManager.Instance?.Play2D(SoundType.QTEStart, 0.35f);

        onSuccess = success;
        onFail = fail;

        needleSpeedDegPerSec = Random.Range(
            Mathf.Min(minNeedleSpeedDegPerSec, maxNeedleSpeedDegPerSec),
            Mathf.Max(minNeedleSpeedDegPerSec, maxNeedleSpeedDegPerSec)
        );

        zoneSizeDegrees = Random.Range(
            Mathf.Min(minZoneDegrees, maxZoneDegrees),
            Mathf.Max(minZoneDegrees, maxZoneDegrees)
        );

        zoneStartClockDeg = PickZoneStart();
        zoneEndClockDeg = Normalize360(zoneStartClockDeg + zoneSizeDegrees);

        needleClockDeg = 0f;

        cycleRemainingDeg = 360f;
        preSpinTimer = Mathf.Max(0f, preSpinDelay);
        spinning = false;

        ApplyZoneVisual();

        if (root != null) root.SetActive(true);
        active = true;

        FreezePlayer(true);

        if (needle != null)
            needle.localRotation = Quaternion.identity;
    }

    private float PickZoneStart()
    {
        if (!limitZoneRange) return Random.Range(0f, 360f);

        float min = Normalize360(zoneStartMinClockDeg);
        float max = Normalize360(zoneStartMaxClockDeg);

        if (min <= max) return Random.Range(min, max);

        float spanA = 360f - min;
        float spanB = max;
        float pick = Random.Range(0f, spanA + spanB);
        return (pick < spanA) ? (min + pick) : (pick - spanA);
    }

    private IEnumerator Resolve(bool success)
    {
        if (!active) yield break;

        active = false;
        spinning = false;

        if (resultFlash != null)
        {
            resultFlash.color = success
                ? new Color(0f, 1f, 0f, 0.35f)
                : new Color(1f, 0f, 0f, 0.35f);

            SetFlashAlpha(resultFlash.color.a);
        }

        yield return new WaitForSecondsRealtime(flashDuration);

        if (root != null) root.SetActive(false);
        SetFlashAlpha(0f);

        FreezePlayer(false);

        if (success) onSuccess?.Invoke();
        else onFail?.Invoke();

        onSuccess = null;
        onFail = null;
    }

    private void FreezePlayer(bool freeze)
    {
        if (playerMovement != null)
            playerMovement.enabled = !freeze;

        if (playerRb != null)
        {
            if (freeze)
            {
                savedLinearDamping = playerRb.linearDamping;
                savedAngularDamping = playerRb.angularDamping;

                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;

                playerRb.linearDamping = 50f;
                playerRb.angularDamping = 50f;
            }
            else
            {
                playerRb.linearDamping = savedLinearDamping;
                playerRb.angularDamping = savedAngularDamping;
            }
        }
    }

    private void ApplyZoneVisual()
    {
        if (zoneImage == null) return;

        zoneImage.fillAmount = zoneSizeDegrees / 360f;
        zoneImage.rectTransform.localRotation = Quaternion.Euler(0f, 0f, -zoneStartClockDeg);
    }

    private static bool IsClockAngleInside(float needle, float start, float end)
    {
        if (start <= end) return needle >= start && needle <= end;
        return needle >= start || needle <= end;
    }

    private static float Normalize360(float a)
    {
        a %= 360f;
        if (a < 0f) a += 360f;
        return a;
    }

    private void SetFlashAlpha(float a)
    {
        if (resultFlash == null) return;
        Color c = resultFlash.color;
        c.a = a;
        resultFlash.color = c;
    }
}
