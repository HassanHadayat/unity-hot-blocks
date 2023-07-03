using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource backgroundMusicSource;
    public AudioSource[] sfxSources;

    private void Awake()
    {
        // Ensure only one instance of the AudioManager exists
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

    public void PlayBackgroundMusic(AudioClip clip)
    {
        backgroundMusicSource.clip = clip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        // Find an available AudioSource to play the SFX
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                return;
            }
        }

        // If all AudioSources are busy, create a new temporary one
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = clip;
        newSource.Play();
        Destroy(newSource, clip.length);
    }
}
