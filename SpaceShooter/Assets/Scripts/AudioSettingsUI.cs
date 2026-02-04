using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle muteToggle;

    [Header("Optional readouts")]
    [SerializeField] private TMP_Text musicValueText;
    [SerializeField] private TMP_Text sfxValueText;

    const string PREF_MUSIC = "vol_music";
    const string PREF_SFX = "vol_sfx";
    const string PREF_MUTE = "mute_all";

    bool suppressEvents;

    void Awake()
    {
        if (!panelRoot) panelRoot = gameObject;
    }

    void Start()
    {
        // Slider ranges (safety)
        if (musicSlider) { musicSlider.minValue = 0f; musicSlider.maxValue = 1f; }
        if (sfxSlider) { sfxSlider.minValue = 0f; sfxSlider.maxValue = 1f; }

        // Hook listeners
        if (musicSlider) musicSlider.onValueChanged.AddListener(OnMusicChanged);
        if (sfxSlider) sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        if (muteToggle) muteToggle.onValueChanged.AddListener(OnMuteChanged);

        // Load saved values into UI (and apply to AudioManager)
        SyncFromPrefsAndApply();
    }

    public void Open()
    {
        if (panelRoot) panelRoot.SetActive(true);
        SyncFromPrefsAndApply(); // ensure UI matches saved values when opening
    }

    public void Close()
    {
        if (panelRoot) panelRoot.SetActive(false);
    }

    void SyncFromPrefsAndApply()
    {
        suppressEvents = true;

        float music = PlayerPrefs.GetFloat(PREF_MUSIC, 0.8f);
        float sfx = PlayerPrefs.GetFloat(PREF_SFX, 0.8f);
        bool mute = PlayerPrefs.GetInt(PREF_MUTE, 0) == 1;

        if (musicSlider) musicSlider.value = music;
        if (sfxSlider) sfxSlider.value = sfx;
        if (muteToggle) muteToggle.isOn = mute;

        UpdateReadouts(music, sfx);

        suppressEvents = false;

        // Apply
        AudioManager.Instance?.SetMusicVolume(music);
        AudioManager.Instance?.SetSfxVolume(sfx);
        AudioManager.Instance?.SetMute(mute);
    }

    void OnMusicChanged(float v)
    {
        if (suppressEvents) return;
        AudioManager.Instance?.SetMusicVolume(v);
        if (musicValueText) musicValueText.text = $"{Mathf.RoundToInt(v * 100f)}%";
    }

    void OnSfxChanged(float v)
    {
        if (suppressEvents) return;
        AudioManager.Instance?.SetSfxVolume(v);
        if (sfxValueText) sfxValueText.text = $"{Mathf.RoundToInt(v * 100f)}%";
    }

    void OnMuteChanged(bool mute)
    {
        if (suppressEvents) return;
        AudioManager.Instance?.SetMute(mute);
    }

    void UpdateReadouts(float music, float sfx)
    {
        if (musicValueText) musicValueText.text = $"{Mathf.RoundToInt(music * 100f)}%";
        if (sfxValueText) sfxValueText.text = $"{Mathf.RoundToInt(sfx * 100f)}%";
    }

    public void SyncFromPrefsAndApplyPublic()
    {
        SyncFromPrefsAndApply();
    }
}
