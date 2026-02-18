using UnityEngine;
using TMPro;

public class DetectionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text detectedText;
    [SerializeField] private float lingerSeconds = 0.25f;

    private float timer;

    private void Awake()
    {
        if (detectedText != null) detectedText.text = "";
    }

    public void PingDetected()
    {
        timer = lingerSeconds;
        if (detectedText != null) detectedText.text = "DETECTED!";
    }

    private void Update()
    {
        if (timer <= 0f) return;

        timer -= Time.unscaledDeltaTime;
        if (timer <= 0f && detectedText != null)
            detectedText.text = "";
    }
}