using UnityEngine;

public class SpotlightDetector : MonoBehaviour
{
    [SerializeField] private float alertGainPerSec = 20f;

    private AlertSystem alertSystem;
    private int playerInsideCount;

    public bool IsDetectingPlayer => playerInsideCount > 0;

    private void Awake()
    {
        alertSystem = FindFirstObjectByType<AlertSystem>();
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void Update()
    {
        if (alertSystem == null) return;
        if (playerInsideCount > 0)
        {
            FindFirstObjectByType<DetectionUI>()?.PingDetected();
            alertSystem.AddAlert(alertGainPerSec * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInsideCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInsideCount = Mathf.Max(0, playerInsideCount - 1);
    }
}
