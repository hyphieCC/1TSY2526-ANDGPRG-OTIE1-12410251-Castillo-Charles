using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private LootTracker lootTracker;
    [SerializeField] private ExtractionController extraction;

    [Header("UI")]
    [SerializeField] private TMP_Text lootText;
    [SerializeField] private TMP_Text extractPromptText;

    private int lastTotal = -1;
    private int lastQuota = -1;
    private bool lastInside;
    private bool lastQuotaMet;

    private void Awake()
    {
        if (lootTracker == null) lootTracker = FindFirstObjectByType<LootTracker>();
        if (extraction == null) extraction = FindFirstObjectByType<ExtractionController>();

        Refresh(force: true);
    }

    private void Update()
    {
        Refresh(force: false);
    }

    private void Refresh(bool force)
    {
        if (lootTracker == null) lootTracker = FindFirstObjectByType<LootTracker>();
        if (extraction == null) extraction = FindFirstObjectByType<ExtractionController>();

        int total = lootTracker != null ? lootTracker.Total : 0;
        int quota = lootTracker != null ? lootTracker.Quota : 0;
        bool quotaMet = lootTracker != null && lootTracker.QuotaMet;
        bool inside = extraction != null && extraction.IsPlayerInside;

        // Update loot text only if changed
        if (force || total != lastTotal || quota != lastQuota)
        {
            if (lootText != null)
                lootText.text = $"Loot: {total}/{quota}";

            lastTotal = total;
            lastQuota = quota;
        }

        // Update extraction prompt only if changed
        if (force || inside != lastInside || quotaMet != lastQuotaMet)
        {
            if (extractPromptText != null)
            {
                if (!inside)
                {
                    extractPromptText.text = "";
                }
                else
                {
                    if (quotaMet)
                        extractPromptText.text = "Press SPACE to Extract";
                    else
                        extractPromptText.text = $"Need more loot ({total}/{quota})";
                }
            }

            lastInside = inside;
            lastQuotaMet = quotaMet;
        }
    }
}
