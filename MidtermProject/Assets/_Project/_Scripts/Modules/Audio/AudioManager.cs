using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    private class SoundEntry
    {
        public SoundType type;
        public AudioClip clip;
    }

    [Header("2D SFX Source")]
    [SerializeField] private AudioSource sfx2D;

    [Header("3D Defaults")]
    [SerializeField] private float default3DVolume = 1f;
    [SerializeField] private float default3DMinDistance = 2f;
    [SerializeField] private float default3DMaxDistance = 18f;

    [Header("Sound Library")]
    [SerializeField] private List<SoundEntry> sounds = new();

    private Dictionary<SoundType, AudioClip> lookup;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (sfx2D == null)
        {
            sfx2D = gameObject.AddComponent<AudioSource>();
            sfx2D.playOnAwake = false;
            sfx2D.spatialBlend = 0f;
        }

        BuildLookup();
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<SoundType, AudioClip>();

        foreach (var e in sounds)
        {
            if (e.clip == null) continue;
            if (!lookup.ContainsKey(e.type))
                lookup.Add(e.type, e.clip);
        }
    }

    private bool TryGetClip(SoundType type, out AudioClip clip)
    {
        clip = null;
        return lookup != null && lookup.TryGetValue(type, out clip) && clip != null;
    }

    public void Play2D(SoundType type, float volume = 1f)
    {
        if (!TryGetClip(type, out var clip)) return;
        sfx2D.PlayOneShot(clip, volume);
    }

    public void Play3D(SoundType type, Vector3 position, float volume = 1f, float minDistance = -1f, float maxDistance = -1f)
    {
        if (!TryGetClip(type, out var clip)) return;

        GameObject temp = new GameObject($"OneShot3D_{type}");
        temp.transform.position = position;

        var a = temp.AddComponent<AudioSource>();
        a.clip = clip;
        a.volume = volume * default3DVolume;
        a.spatialBlend = 1f;
        a.rolloffMode = AudioRolloffMode.Logarithmic;
        a.minDistance = (minDistance > 0f) ? minDistance : default3DMinDistance;
        a.maxDistance = (maxDistance > 0f) ? maxDistance : default3DMaxDistance;

        a.Play();
        Destroy(temp, clip.length + 0.1f);
    }
}
