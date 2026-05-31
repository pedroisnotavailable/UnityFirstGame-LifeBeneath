using UnityEngine;
using UnityEngine.Audio;

public class GameAudioManager : MonoBehaviour
{
    private static GameAudioManager instance;

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup ambientGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Looping Audio")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip ambientClip;
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 0.6f;
    [Range(0f, 1f)] [SerializeField] private float ambientVolume = 0.45f;

    [Header("Behaviour")]
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool keepBetweenScenes = true;

    private AudioSource musicSource;
    private AudioSource ambientSource;

    private void Awake()
    {
        if (keepBetweenScenes)
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        musicSource = CreateLoopingSource("Music", musicClip, musicGroup, musicVolume);
        ambientSource = CreateLoopingSource("Ambient", ambientClip, ambientGroup, ambientVolume);

        if (playOnAwake)
        {
            PlayLoops();
        }
    }

    public void PlayLoops()
    {
        PlaySource(musicSource);
        PlaySource(ambientSource);
    }

    public void StopLoops()
    {
        StopSource(musicSource);
        StopSource(ambientSource);
    }

    public void SetMasterVolume(float volume)
    {
        SetMixerVolume("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetMixerVolume("MusicVolume", volume);
    }

    public void SetAmbientVolume(float volume)
    {
        SetMixerVolume("AmbientVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        SetMixerVolume("SFXVolume", volume);
    }

    public void PlaySfx(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null)
        {
            return;
        }

        GameObject audioObject = new GameObject("One Shot SFX");
        audioObject.transform.position = position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = sfxGroup;
        audioSource.volume = volume;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 25f;
        audioSource.Play();

        Destroy(audioObject, clip.length);
    }

    private AudioSource CreateLoopingSource(string sourceName, AudioClip clip, AudioMixerGroup mixerGroup, float volume)
    {
        GameObject sourceObject = new GameObject(sourceName);
        sourceObject.transform.SetParent(transform);

        AudioSource audioSource = sourceObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        return audioSource;
    }

    private void PlaySource(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void StopSource(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void SetMixerVolume(string parameterName, float volume)
    {
        if (audioMixer == null)
        {
            return;
        }

        float clampedVolume = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat(parameterName, Mathf.Log10(clampedVolume) * 20f);
    }
}
