using UnityEngine;

public class GuardVision : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform eyes;
    [SerializeField] private GameObject alertIcon;
    [SerializeField] private Renderer coneRenderer;

    [Header("Vision")]
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 70f;
    [SerializeField] private LayerMask obstructionMask = ~0;
    [SerializeField] private float checkInterval = 0.1f;

    [Header("Alert")]
    [SerializeField] private float alertGainPerSec = 35f;
    [SerializeField] private float graceBeforeAlert = 0.2f;

    [Header("Cone Tint")]
    [SerializeField] private Color coneNormalColor = new Color(1f, 1f, 1f, 0.20f);
    [SerializeField] private Color coneDetectedColor = new Color(1f, 0f, 0f, 0.25f);

    private Transform player;
    private AlertSystem alertSystem;
    private float timer;
    private bool canSee;
    private bool wasSeeing;
    private float seenTimer;

    private MaterialPropertyBlock mpb;
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    public float ViewDistance => viewDistance;
    public float ViewAngle => viewAngle;
    public bool IsSeeingPlayer => canSee;

    private void Awake()
    {
        if (eyes == null) eyes = transform;

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        alertSystem = FindFirstObjectByType<AlertSystem>();

        if (coneRenderer == null)
            coneRenderer = GetComponentInChildren<Renderer>();

        mpb = new MaterialPropertyBlock();

        SetAlertIcon(false);
        SetConeTint(false);
    }

    private void Update()
    {
        if (player == null || alertSystem == null) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = checkInterval;
            canSee = CanSeePlayer();
            if (canSee && !wasSeeing)
            {
                AudioManager.Instance?.Play3D(SoundType.Detected, eyes.position, 0.9f);
            }
            wasSeeing = canSee;

            if (!canSee)
                seenTimer = 0f;

            SetAlertIcon(canSee);
            SetConeTint(canSee);
        }

        if (canSee)
        {
            seenTimer += Time.deltaTime;

            if (seenTimer >= graceBeforeAlert)
                alertSystem.AddAlert(alertGainPerSec * Time.deltaTime);
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 to = player.position - eyes.position;
        float dist = to.magnitude;
        if (dist > viewDistance) return false;

        Vector3 dir = to / Mathf.Max(dist, 0.0001f);
        Vector3 fwd = eyes.forward;

        dir.y = 0f;
        fwd.y = 0f;
        dir.Normalize();
        fwd.Normalize();

        float angle = Vector3.Angle(fwd, dir);
        if (angle > viewAngle * 0.5f) return false;

        Vector3 origin = eyes.position;
        Vector3 target = player.position + Vector3.up * 0.9f;
        Vector3 rayDir = target - origin;
        float rayDist = rayDir.magnitude;
        if (rayDist <= 0.01f) return true;

        rayDir /= rayDist;

        if (Physics.Raycast(origin, rayDir, out RaycastHit hit, rayDist, obstructionMask, QueryTriggerInteraction.Ignore))
        {
            if (!hit.transform.CompareTag("Player") && hit.transform.root != player)
                return false;
        }

        return true;
    }

    private void SetAlertIcon(bool on)
    {
        if (alertIcon != null && alertIcon.activeSelf != on)
        {
            alertIcon.SetActive(on);
            FindFirstObjectByType<DetectionUI>()?.PingDetected();
        }
    }

    private void SetConeTint(bool detected)
    {
        if (coneRenderer == null) return;

        Color c = detected ? coneDetectedColor : coneNormalColor;

        coneRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(BaseColorId, c);
        mpb.SetColor(ColorId, c);
        coneRenderer.SetPropertyBlock(mpb);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (eyes == null) return;

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(eyes.position, viewDistance);

    //    Vector3 left = Quaternion.Euler(0f, -viewAngle * 0.5f, 0f) * eyes.forward;
    //    Vector3 right = Quaternion.Euler(0f, viewAngle * 0.5f, 0f) * eyes.forward;

    //    Gizmos.DrawLine(eyes.position, eyes.position + left * viewDistance);
    //    Gizmos.DrawLine(eyes.position, eyes.position + right * viewDistance);
    //}
}
