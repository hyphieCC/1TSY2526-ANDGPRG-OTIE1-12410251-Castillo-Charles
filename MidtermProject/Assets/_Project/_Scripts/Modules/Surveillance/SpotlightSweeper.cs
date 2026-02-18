using UnityEngine;

public class SpotlightSweeper : MonoBehaviour
{
    [SerializeField] private float yawDegrees = 60f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float startOffset = 0f;

    private Vector3 baseForward;

    private void Awake()
    {
        Vector3 f = transform.forward;
        f.y = 0f;
        if (f.sqrMagnitude < 0.0001f) f = Vector3.forward;
        baseForward = f.normalized;
    }

    private void Update()
    {
        float s = Mathf.Sin((Time.time * speed + startOffset) * Mathf.PI * 2f);
        float yaw = s * yawDegrees;

        Quaternion rot = Quaternion.AngleAxis(yaw, Vector3.up);

        Vector3 dir = rot * baseForward;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
