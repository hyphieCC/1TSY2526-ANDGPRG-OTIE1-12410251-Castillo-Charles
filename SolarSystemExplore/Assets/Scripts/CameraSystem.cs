using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform target;

    public float farHeight;
    public float nearHeight;

    public float followSpeed;
    public float zoomSpeed;

    private float currentHeight;
    private float desiredHeight;

    void Start()
    {
        currentHeight = farHeight;
        desiredHeight = farHeight;
    }

    void LateUpdate()
    {
        if (!target) return;

        float dt = Time.unscaledDeltaTime;

        currentHeight = Mathf.Lerp(currentHeight, desiredHeight, zoomSpeed * dt);

        Vector3 desiredPos = target.position + Vector3.up * currentHeight;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * dt);

        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    public void SetNear(bool near)
    {
        desiredHeight = near ? nearHeight : farHeight;
    }
}