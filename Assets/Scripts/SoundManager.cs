using UnityEngine;
using UnityEngine.Audio;

public enum AudioGroup
{
    Main,
    SFX,
    Music
};

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public const string MainLevelSavedDataString = "Main_Level";
    public const string SFXLevelSavedDataString = "SFX_Level";
    public const string MusicLevelSavedDataString = "Music_Level";

    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private AudioSource audioSourceOneShot;

    [SerializeField] private AudioMixerGroup MainGroup;
    [SerializeField] private AudioMixerGroup SFXGroup;
    [SerializeField] private AudioMixerGroup MusicGroup;

    [SerializeField] private AudioMixer mainMixer;
    private float soundsVolume;
    private float musicVolume;
    private float mainSoundsVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadSoundSettings();
    }

    public AudioSource PlaySound(AudioClip[] clip, Vector3 position, AudioMixerGroup mixerGroup = default, bool isLooping = false, float volume = 1)
    {
        return PlaySound(clip[Random.Range(0, clip.Length)], position, mixerGroup, isLooping, volume);
    }

    public AudioSource PlaySound(AudioClip clip, Vector3 position, AudioMixerGroup mixerGroup = default, bool isLooping = false, float volume = 1)
    {
        if (clip == null) return null;

        AudioSource audio = Instantiate(audioSourcePrefab, position, Quaternion.identity);
        audio.name = "SoundObject";
        audio.loop = isLooping;
        audio.clip = clip;
        audio.spatialBlend = 1f;
        audio.dopplerLevel = 0f;
        audio.outputAudioMixerGroup = mixerGroup;
        audio.volume = volume;
        audio.Play();
        if (!isLooping) Destroy(audio.gameObject, clip.length);

        return audio;
    }

    public AudioSource PlaySound(AudioClip[] clip, Transform attachedTransform, AudioMixerGroup mixerGroup = default, bool isLooping = false, float volume = 1)
    {
        return PlaySound(clip[Random.Range(0, clip.Length)], attachedTransform, mixerGroup, isLooping, volume);
    }

    public AudioSource PlaySound(AudioClip clip, Transform attachedTransform, AudioMixerGroup mixerGroup = default, bool isLooping = false, float volume = 1)
    {
        if (clip == null) return null;

        AudioSource audio = Instantiate(audioSourcePrefab, attachedTransform);
        audio.name = "SoundObject";
        audio.loop = isLooping;
        audio.clip = clip;
        audio.spatialBlend = 1f;
        audio.dopplerLevel = 0f;
        audio.outputAudioMixerGroup = mixerGroup;
        audio.volume = volume;
        audio.Play();
        if (!isLooping) Destroy(audio.gameObject, clip.length);

        return audio;
    }

    public void PlaySoundOneShot(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSourceOneShot == null)
        {
            audioSourceOneShot = Camera.main.GetComponentInChildren<AudioSource>();
        }

        audioSourceOneShot.PlayOneShot(clip);
    }

    public AudioMixerGroup GetAudioMixerGroup(AudioGroup audioGroup)
    {
        return audioGroup switch
        {
            AudioGroup.SFX => SFXGroup,
            AudioGroup.Music => MusicGroup,
            _ => MainGroup,
        };
    }

    #region Sound Options
    private void LoadSoundSettings()
    {
        SetMainMixerGroupVolume("MainVolume", PlayerPrefs.GetFloat(MainLevelSavedDataString, 0.8f));
        SetMainMixerGroupVolume("SFXVolume", PlayerPrefs.GetFloat(SFXLevelSavedDataString, 1));
        SetMainMixerGroupVolume("MusicVolume", PlayerPrefs.GetFloat(MusicLevelSavedDataString, 1));
    }

    public void SetMainSoundVolume(float value)
    {
        mainSoundsVolume = value;
        SetMainMixerGroupVolume("MainVolume", value);
        PlayerPrefs.SetFloat(MainLevelSavedDataString, value);
    }

    public float GetMainSoundVolume()
    {
        return mainSoundsVolume;
    }

    public void SetSFXVolume(float value)
    {
        soundsVolume = value;
        SetMainMixerGroupVolume("SFXVolume", value);
        PlayerPrefs.SetFloat(SFXLevelSavedDataString, value);
    }

    public float GetSoundVolume()
    {
        return soundsVolume;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        SetMainMixerGroupVolume("MusicVolume", value);
        PlayerPrefs.SetFloat(MusicLevelSavedDataString, value);
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    /// <summary>
    /// changes the volume of the main mixer group
    /// </summary>
    /// <param name="name">exposed volume parameter name</param>
    /// <param name="value">from 0.0001 to 1. (Cant be 0)</param>
    public void SetMainMixerGroupVolume(string name, float value)
    {
        mainMixer.SetFloat(name, Mathf.Log10(value) * 20);
    }
    #endregion
}
