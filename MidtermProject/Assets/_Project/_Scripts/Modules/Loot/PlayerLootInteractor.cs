using UnityEngine;

public class PlayerLootInteractor : MonoBehaviour
{
    [SerializeField] private KeyCode stealKey = KeyCode.F;
    [SerializeField] private StealPromptUI stealPromptUI;

    [Header("QTE")]
    [SerializeField] private QTESkillCheck qte;
    [SerializeField] private float qteFailAlertSpike = 30f;

    [Header("Line of Sight")]
    [SerializeField] private bool requireLineOfSight = true;
    [SerializeField] private LayerMask lineOfSightMask = ~0;
    [SerializeField] private Transform rayOrigin;

    [Header("Optional")]
    [SerializeField] private bool requireFacing = true;
    [SerializeField] private float maxAngle = 50f;

    private LootItem current;
    private bool qteRunning;

    private void Awake()
    {
        if (rayOrigin == null) rayOrigin = transform;
        if (qte == null) qte = FindFirstObjectByType<QTESkillCheck>();
    }

    private void Update()
    {
        UpdateStealPrompt();

        if (current == null || current.IsStolen) return;

        if (Input.GetKeyDown(stealKey))
        {
            if (qteRunning) return;

            if (!requireFacing || IsFacing(current.transform))
            {
                if (!requireLineOfSight || HasLineOfSight(current.transform))
                {
                    if (qte == null) qte = FindFirstObjectByType<QTESkillCheck>();
                    if (qte == null)
                    {
                        Debug.LogError("PlayerLootInteractor: No QTESkillCheck found in scene.");
                        return;
                    }

                    qteRunning = true;

                    LootItem lootToSteal = current;
                    qte.StartSkillCheck(
                        success: () =>
                        {
                            var tracker = FindFirstObjectByType<LootTracker>();
                            if (tracker != null) tracker.AddLoot(lootToSteal.Value, lootToSteal.Name);

                            AudioManager.Instance?.Play2D(SoundType.LootSuccess, 1f);
                            lootToSteal.Steal();

                            if (current == lootToSteal) current = null;

                            qteRunning = false;
                        },
                        fail: () =>
                        {
                            var alert = FindFirstObjectByType<AlertSystem>();
                            AudioManager.Instance?.Play2D(SoundType.LootFail, 1f);
                            if (alert != null) alert.AddAlert(qteFailAlertSpike);

                            qteRunning = false;
                        }
                    );
                }
            }
        }
    }

    private bool IsFacing(Transform t)
    {
        Vector3 to = t.position - transform.position;
        to.y = 0f;
        if (to.sqrMagnitude < 0.0001f) return true;

        float a = Vector3.Angle(transform.forward, to.normalized);
        return a <= maxAngle;
    }

    private bool HasLineOfSight(Transform loot)
    {
        if (rayOrigin == null) return true;

        Vector3 origin = rayOrigin.position;
        Vector3 target = loot.position;

        Vector3 dir = target - origin;
        float dist = dir.magnitude;
        if (dist <= 0.01f) return true;

        dir /= dist;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, dist, lineOfSightMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform != loot && hit.transform.root != loot) return false;
        }

        return true;
    }

    private void UpdateStealPrompt()
    {
        if (stealPromptUI == null) return;

        if (qteRunning || current == null || current.IsStolen)
        {
            stealPromptUI.Clear();
            return;
        }

        if (requireFacing && !IsFacing(current.transform))
        {
            stealPromptUI.Show("Face the item to steal");
            return;
        }

        if (requireLineOfSight && !HasLineOfSight(current.transform))
        {
            stealPromptUI.Show("Line of sight blocked");
            return;
        }

        stealPromptUI.Show($"Press {stealKey} to Steal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<LootItem>(out var loot) && !loot.IsStolen)
            current = loot;
    }

    private void OnTriggerExit(Collider other)
    {
        if (current != null && other.gameObject == current.gameObject)
            current = null;
    }
}
