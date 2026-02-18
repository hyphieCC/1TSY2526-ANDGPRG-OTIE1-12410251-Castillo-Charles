using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExtractionController : MonoBehaviour
{
    [SerializeField] private KeyCode extractKey = KeyCode.Space;

    [Header("Zone Visual")]
    [SerializeField] private Renderer zoneRenderer;
    [SerializeField] private Color underQuotaColor = new Color(1f, 1f, 0f, 0.25f);
    [SerializeField] private Color readyColor = new Color(0f, 1f, 0f, 0.25f);
    [SerializeField] private bool autoFitVisualToCollider = true;
    [SerializeField] private float visualScaleInset = 0.98f;

    private bool playerInside;
    private LootTracker tracker;

    private MaterialPropertyBlock mpb;
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    private void Awake()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        tracker = FindFirstObjectByType<LootTracker>();

        if (zoneRenderer == null)
            zoneRenderer = GetComponentInChildren<Renderer>();

        mpb = new MaterialPropertyBlock();

        if (autoFitVisualToCollider)
            FitVisualToCollider();

        UpdateZoneColor();
    }

    private void Update()
    {
        if (tracker == null) tracker = FindFirstObjectByType<LootTracker>();

        UpdateZoneColor();

        if (!playerInside) return;

        if (Input.GetKeyDown(extractKey))
        {
            if (tracker == null)
            {
                Debug.LogError("ExtractionController: No LootTracker in scene.");
                return;
            }

            if (!tracker.QuotaMet)
            {
                Debug.Log($"Cannot extract: {tracker.Total}/{tracker.Quota}");
                return;
            }

            var flow = FindFirstObjectByType<WinLoseFlow>();
            if (flow != null) flow.Win();
            else Debug.Log("WIN (no WinLoseFlow yet).");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    public bool CanExtractNow => playerInside && tracker != null && tracker.QuotaMet;

    private void UpdateZoneColor()
    {
        if (zoneRenderer == null) return;

        bool met = tracker != null && tracker.QuotaMet;
        Color c = met ? readyColor : underQuotaColor;

        zoneRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(BaseColorId, c);
        mpb.SetColor(ColorId, c);
        zoneRenderer.SetPropertyBlock(mpb);
    }

    private void FitVisualToCollider()
    {
        if (zoneRenderer == null) return;

        Collider col = GetComponent<Collider>();
        Transform vis = zoneRenderer.transform;

        if (col is BoxCollider box)
        {
            vis.localPosition = box.center;
            vis.localRotation = Quaternion.identity;
            vis.localScale = box.size * visualScaleInset;
        }
        else
        {
            Bounds b = col.bounds;
            vis.position = b.center;
            vis.rotation = Quaternion.identity;
            vis.localScale = b.size * visualScaleInset;
        }
    }

    public bool IsPlayerInside => playerInside;
}