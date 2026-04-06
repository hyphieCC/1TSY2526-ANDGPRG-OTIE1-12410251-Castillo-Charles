using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public enum SFXType
    {
        ArrowShoot,
        CannonShoot,
        IceShoot,
        FireShoot,
        ArrowHit,
        CannonHit,
        IceHit,
        FireHit,
        SlimeHit1,
        SlimeHit2,
        RavenHit1,
        RavenHit2,
        GoblinHit1,
        GoblinHit2,
        SlimeDeath,
        RavenDeath,
        GoblinDeath,
        CoreHit,
        CoreDeath,
        CanClick,
        CannotClick,
        UpgradeSuccess,
        BuildSuccess,
        GoldGained
    }

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("SFX Clips")]
    [SerializeField] AudioClip[] sfxClips;

    [Header("Volume")]
    [Range(0f, 1f)][SerializeField] float masterVolume = 1f;
    [Range(0f, 1f)][SerializeField] float musicVolume = 1f;
    [Range(0f, 1f)][SerializeField] float sfxVolume = 1f;

    [Header("3D Audio")]
    [SerializeField] float spatialBlend3D = 1f;
    [SerializeField] float minDistance = 5f;
    [SerializeField] float maxDistance = 40f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        ApplyVolumes();
    }

    public void PlaySFX(SFXType type)
    {
        int index = (int)type;

        AudioClip clip = sfxClips[index];

        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, masterVolume * sfxVolume);
        }
    }

    public void PlaySFX3D(SFXType type, Vector3 position)
    {
        int index = (int)type;

        AudioClip clip = sfxClips[index];

        if (clip == null)
        {
            return;
        }

        GameObject tempAudioObject = new GameObject("Temp3DAudio_" + type);
        tempAudioObject.transform.position = position;

        AudioSource tempSource = tempAudioObject.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.volume = masterVolume * sfxVolume;
        tempSource.spatialBlend = spatialBlend3D;
        tempSource.minDistance = minDistance;
        tempSource.maxDistance = maxDistance;
        tempSource.rolloffMode = AudioRolloffMode.Logarithmic;
        tempSource.Play();

        Destroy(tempAudioObject, clip.length + 0.1f);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
        ApplyVolumes();
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        ApplyVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        ApplyVolumes();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        ApplyVolumes();
    }

    public float GetMasterVolume() => masterVolume;
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;

    void ApplyVolumes()
    {
        musicSource.volume = masterVolume * musicVolume;
        sfxSource.volume = masterVolume * sfxVolume;
    }
}