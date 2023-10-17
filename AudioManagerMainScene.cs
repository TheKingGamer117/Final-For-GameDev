using UnityEngine;

public class AudioManagerMainScene : MonoBehaviour
{
    public static AudioManagerMainScene instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip gameMusic;
    public AudioClip shootSFX;

    [Range(0f, 1f)] public float musicVolume = 1f;  // Set default music volume to max
    [Range(0f, 1f)] public float sfxVolume = 1f;    // Set default SFX volume to max

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Apply the default volume settings
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
        PlayGameMusic();
    }

    public void PlayGameMusic()
    {
        musicSource.clip = gameMusic;
        musicSource.Play();
    }

    public void PlayShootSFX()
    {
        sfxSource.PlayOneShot(shootSFX);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume;
    }
}
