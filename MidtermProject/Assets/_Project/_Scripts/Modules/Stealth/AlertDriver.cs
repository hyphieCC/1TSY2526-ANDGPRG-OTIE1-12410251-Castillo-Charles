using UnityEngine;

public class AlertDriver : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private AlertSystem alertSystem;

    private void Awake()
    {
        if (playerMovement == null) playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (alertSystem == null) alertSystem = FindFirstObjectByType<AlertSystem>();
    }

    private void Update()
    {
        if (playerMovement == null || alertSystem == null) return;

        alertSystem.Tick(
            playerMovement.CurrentMode,
            playerMovement.SpeedNormalized,
            Time.deltaTime
        );
    }
}
