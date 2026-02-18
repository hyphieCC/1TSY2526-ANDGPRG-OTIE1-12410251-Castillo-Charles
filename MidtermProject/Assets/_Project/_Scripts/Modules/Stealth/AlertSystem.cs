using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    [Header("Alert Range")]
    [SerializeField] private float maxAlert = 100f;

    [Header("Gain Rates (per second)")]
    [SerializeField] private float walkGainPerSec = 8f;
    [SerializeField] private float runGainPerSec = 25f;

    [Header("Decay")]
    [Tooltip("How many seconds must pass without gaining alert before decay begins.")]
    [SerializeField] private float decayDelaySeconds = 2f;
    [SerializeField] private float decayPerSec = 10f;

    [Header("Movement")]
    [Tooltip("SpeedNormalized must exceed this to count as moving.")]
    [SerializeField] private float movementThreshold = 0.05f;

    [Header("Warning / Lose Thresholds")]
    [SerializeField] private float warningThreshold = 90f;

    [Header("Debug")]
    [SerializeField] private bool debugAlert = true;
    [SerializeField] private float debugLogInterval = 0.5f;

    public float Alert { get; private set; }
    public float Alert01 => maxAlert <= 0.01f ? 0f : Mathf.Clamp01(Alert / maxAlert);

    public bool IsLost => lost;

    private float lastGainTime;
    private bool lost;
    private bool warningShown;

    private float debugTimer;

    private void Awake()
    {
        Alert = 0f;
        lastGainTime = Time.time;
        warningShown = false;
        lost = false;
        debugTimer = 0f;
    }

    public void AddAlert(float amount)
    {
        if (lost) return;

        Alert = Mathf.Clamp(Alert + amount, 0f, maxAlert);

        if (amount > 0f)
            lastGainTime = Time.time;

        CheckWarningAndLose();
    }

    public void Tick(PlayerMovement.MoveMode mode, float speedNormalized, float dt)
    {
        if (lost) return;

        bool moving = speedNormalized > movementThreshold;

        float gainPerSec = 0f;
        if (moving)
        {
            if (mode == PlayerMovement.MoveMode.Run) gainPerSec = runGainPerSec;
            else if (mode == PlayerMovement.MoveMode.Walk) gainPerSec = walkGainPerSec;
            else gainPerSec = 0f;
        }

        if (gainPerSec > 0f)
        {
            Alert = Mathf.Clamp(Alert + gainPerSec * dt, 0f, maxAlert);
            lastGainTime = Time.time;
        }
        else
        {
            if (Time.time - lastGainTime >= decayDelaySeconds)
            {
                Alert = Mathf.Max(0f, Alert - decayPerSec * dt);
            }
        }

        CheckWarningAndLose();
        HandleDebug(dt);
    }

    private void CheckWarningAndLose()
    {
        if (lost) return;

        if (!warningShown && Alert >= warningThreshold && Alert < maxAlert)
        {
            warningShown = true;
            Debug.Log("WARNING: ALERT VERY HIGH! STOP MOVING!");
        }

        if (Alert >= maxAlert)
        {
            Lose();
            return;
        }

        if (warningShown && Alert < warningThreshold)
        {
            warningShown = false;
        }
    }

    private void Lose()
    {
        if (lost) return;
        lost = true;
        Debug.Log("YOU LOST: Alert reached 100!");
    }

    private void HandleDebug(float dt)
    {
        if (!debugAlert) return;

        debugTimer += dt;
        if (debugTimer >= debugLogInterval)
        {
            debugTimer = 0f;
            Debug.Log($"[ALERT] {Alert:F1} / {maxAlert} ({Alert01:F2})");
        }
    }
}
