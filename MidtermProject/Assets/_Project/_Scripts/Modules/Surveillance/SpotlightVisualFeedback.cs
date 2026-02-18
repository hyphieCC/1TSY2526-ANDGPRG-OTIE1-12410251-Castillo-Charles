using UnityEngine;

public class SpotlightVisualFeedback : MonoBehaviour
{
    [SerializeField] private SpotlightDetector detector;
    [SerializeField] private Renderer beamRenderer;

    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color(1f, 1f, 0.8f, 0.25f);
    [SerializeField] private Color detectedColor = new Color(1f, 0f, 0f, 0.30f);

    private MaterialPropertyBlock mpb;
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private bool last;

    private void Awake()
    {
        if (detector == null) detector = GetComponentInParent<SpotlightDetector>();
        if (beamRenderer == null) beamRenderer = GetComponentInChildren<Renderer>();

        mpb = new MaterialPropertyBlock();

        SetTint(false);
        last = false;
    }

    private void Update()
    {
        if (detector == null || beamRenderer == null) return;

        bool detecting = detector.IsDetectingPlayer;
        if (detecting == last) return;

        if (detecting)
        {
            AudioManager.Instance?.Play3D(SoundType.SpotlightDetected, transform.position, 0.8f);
        }

        SetTint(detecting);
        last = detecting;
    }

    private void SetTint(bool detected)
    {
        Color c = detected ? detectedColor : normalColor;

        beamRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(BaseColorId, c);
        mpb.SetColor(ColorId, c);
        beamRenderer.SetPropertyBlock(mpb);
    }
}
