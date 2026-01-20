using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    public Transform sun;
    public float orbitSpeed = 10f;
    public Vector3 orbitAxis = Vector3.up;

    public CameraSystem cameraZoom;

    public float slowTimeScale = 0.3f;

    void Update()
    {
        if (sun != null)
        {
            transform.RotateAround(sun.position, orbitAxis, orbitSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (cameraZoom)
            cameraZoom.SetNear(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (cameraZoom)
            cameraZoom.SetNear(false);
    }
}