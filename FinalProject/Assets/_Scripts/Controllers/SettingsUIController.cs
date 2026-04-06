using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsUIController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] GameObject settingsPanel;

    [Header("Audio Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Header("Level Buttons")]
    [SerializeField] string level1SceneName = "Level1";
    [SerializeField] string level2SceneName = "Level2";
    [SerializeField] string level3SceneName = "Level3";

    void Start()
    {
        settingsPanel.SetActive(false);

        masterSlider.value = SoundManager.Instance.GetMasterVolume();
        musicSlider.value = SoundManager.Instance.GetMusicVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();

        masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OnMasterVolumeChanged(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(level1SceneName);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(level2SceneName);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene(level3SceneName);
    }
}