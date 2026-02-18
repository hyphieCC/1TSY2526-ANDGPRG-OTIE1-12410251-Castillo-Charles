using UnityEngine;

public class FootstepEmitter : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;

    [Header("Timing")]
    [SerializeField] private float walkStepInterval = 0.50f;
    [SerializeField] private float runStepInterval = 0.33f;

    [Header("3D Settings")]
    [SerializeField] private float volume = 0.45f;

    private float timer;

    private void Awake()
    {
        if (movement == null) movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (movement == null) return;

        float speed01 = movement.SpeedNormalized;
        if (speed01 < 0.15f)
        {
            timer = 0f;
            return;
        }

        if (movement.CurrentMode == PlayerMovement.MoveMode.Crawl)
        {
            timer = 0f;
            return;
        }

        float interval = movement.CurrentMode == PlayerMovement.MoveMode.Run ? runStepInterval : walkStepInterval;
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;

            SoundType type = movement.CurrentMode == PlayerMovement.MoveMode.Run
                ? SoundType.FootstepRun
                : SoundType.FootstepWalk;

            AudioManager.Instance?.Play3D(type, transform.position, volume);
        }
    }
}
