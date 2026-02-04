using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string musicVolParam = "MusicVol";
    [SerializeField] private string sfxVolParam = "SfxVol";

    [Header("Clips")]
    public AudioClip bgmClip;
    public AudioClip shootClip;
    public AudioClip enemyDeathClip;
    public AudioClip playerDeathClip;
    public AudioClip powerUpClip;
    public AudioClip levelCompleteClip;

    const string PREF_MUSIC = "vol_music";
    const string PREF_SFX = "vol_sfx";
    const string PREF_MUTE = "mute_all";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load settings
        float music = PlayerPrefs.GetFloat(PREF_MUSIC, 0.8f);
        float sfx = PlayerPrefs.GetFloat(PREF_SFX, 0.8f);
        int mute = PlayerPrefs.GetInt(PREF_MUTE, 0);

        SetMusicVolume(music);
        SetSfxVolume(sfx);
        SetMute(mute == 1);
    }

    void Start()
    {
        if (bgmClip && musicSource)
        {
            musicSource.clip = bgmClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        if (!clip || !sfxSource) return;
        sfxSource.PlayOneShot(clip);
    }

    // Slider input should be 0..1
    public void SetMusicVolume(float v01)
    {
        v01 = Mathf.Clamp01(v01);
        mixer.SetFloat(musicVolParam, ToDb(v01));
        PlayerPrefs.SetFloat(PREF_MUSIC, v01);
    }

    public void SetSfxVolume(float v01)
    {
        v01 = Mathf.Clamp01(v01);
        mixer.SetFloat(sfxVolParam, ToDb(v01));
        PlayerPrefs.SetFloat(PREF_SFX, v01);
    }

    public void SetMute(bool mute)
    {
        // Hard mute by setting both to -80 dB
        if (mute)
        {
            mixer.SetFloat(musicVolParam, -80f);
            mixer.SetFloat(sfxVolParam, -80f);
            PlayerPrefs.SetInt(PREF_MUTE, 1);
        }
        else
        {
            // Restore saved volumes
            SetMusicVolume(PlayerPrefs.GetFloat(PREF_MUSIC, 0.8f));
            SetSfxVolume(PlayerPrefs.GetFloat(PREF_SFX, 0.8f));
            PlayerPrefs.SetInt(PREF_MUTE, 0);
        }
    }

    static float ToDb(float v01)
    {
        // 0 -> -80 dB (silent), 1 -> 0 dB
        if (v01 <= 0.0001f) return -80f;
        return Mathf.Log10(v01) * 20f;
    }
}
