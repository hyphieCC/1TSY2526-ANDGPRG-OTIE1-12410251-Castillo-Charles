using UnityEngine;
using UnityEngine.UI;

public class AlertUI : MonoBehaviour
{
    [SerializeField] private AlertSystem alertSystem;

    [Header("UI Refs")]
    [SerializeField] private RectTransform leftFill;
    [SerializeField] private RectTransform rightFill;
    [SerializeField] private RectTransform barArea; // the full bar width reference
    [SerializeField] private Image warningOverlay;  // optional (can be null)

    [Header("Behavior")]
    [SerializeField] private float smooth = 12f; // higher = snappier

    private float current01;

    private void Awake()
    {
        if (alertSystem == null) alertSystem = FindFirstObjectByType<AlertSystem>();
        current01 = 0f;
        ApplyFill(0f);
    }

    private void Update()
    {
        if (alertSystem == null || barArea == null) return;

        float target01 = alertSystem.Alert01;
        current01 = Mathf.Lerp(current01, target01, 1f - Mathf.Exp(-smooth * Time.deltaTime));

        ApplyFill(current01);

        // Optional: flash overlay when high alert (you can wire this later)
        if (warningOverlay != null)
        {
            warningOverlay.enabled = alertSystem.Alert >= 90f && !alertSystem.IsLost;
        }
    }

    private void ApplyFill(float alert01)
    {
        float halfWidth = barArea.rect.width * 0.5f;
        float fillWidth = halfWidth * Mathf.Clamp01(alert01);

        SetWidth(leftFill, fillWidth);
        SetWidth(rightFill, fillWidth);
    }

    private static void SetWidth(RectTransform rt, float w)
    {
        if (rt == null) return;
        Vector2 size = rt.sizeDelta;
        size.x = w;
        rt.sizeDelta = size;
    }
}
