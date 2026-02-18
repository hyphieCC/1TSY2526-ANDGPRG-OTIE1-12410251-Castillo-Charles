using UnityEngine;

public class GuardVisionConeVisual : MonoBehaviour
{
    [SerializeField] private GuardVision vision;
    [SerializeField] private Transform coneVisual;

    [Tooltip("If your cone mesh points along +Z, keep this true.")]
    [SerializeField] private bool conePointsForwardZ = true;

    private void Awake()
    {
        if (vision == null) vision = GetComponentInParent<GuardVision>();
        if (coneVisual == null) coneVisual = transform;

        Apply();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) Apply();
    }

    public void Apply()
    {
        if (vision == null || coneVisual == null) return;

        float dist = vision.ViewDistance;
        float angle = vision.ViewAngle;

        float radius = Mathf.Tan((angle * 0.5f) * Mathf.Deg2Rad) * dist;
        float diameter = radius * 2f;

        Vector3 s = coneVisual.localScale;

        if (conePointsForwardZ)
        {
            s.z = dist;
            s.x = diameter;
            s.y = diameter;

            coneVisual.localPosition = new Vector3(0f, 0f, dist * 0.5f);
        }
        else
        {
            s.y = dist;
            s.x = diameter;
            s.z = diameter;

            coneVisual.localPosition = new Vector3(0f, dist * 0.5f, 0f);
        }

        coneVisual.localScale = s;
    }
}
