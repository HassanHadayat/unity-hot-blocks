using System.Collections.Generic;
using UnityEngine;

public enum SfxAudioClip { bubblePop, blockPlacing, blockCracking, blockBreaking, rowComplete, rowCompleteCombo, powerup }
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource backgroundMusicSource;
    public AudioSource UISource;
    public AudioSource[] sfxSources;

    public AudioClip bgMusic;
    public AudioClip UIClip;
    public AudioClip[] sfxClips;
    private Dictionary<SfxAudioClip, AudioClip> sfxAudioClips;
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
    private void Start()
    {

        if (PlayerPrefs.GetInt("isMusic", 1) == 1)
        {
            PlayBackgroundMusic();
        }

        sfxAudioClips = new Dictionary<SfxAudioClip, AudioClip>
        {
            { SfxAudioClip.bubblePop, sfxClips[0] },
            { SfxAudioClip.blockPlacing, sfxClips[1] },
            { SfxAudioClip.blockCracking, sfxClips[2] },
            { SfxAudioClip.blockBreaking, sfxClips[3] },
            { SfxAudioClip.rowComplete, sfxClips[4] },
            { SfxAudioClip.rowCompleteCombo, sfxClips[5] },
            { SfxAudioClip.powerup, sfxClips[6] }
        };
    }
    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.clip = bgMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
    }

    public void PlaySFX(SfxAudioClip sfxClip)
    {
        if (PlayerPrefs.GetInt("isSound", 1) == 0)
            return;

        // Find an available AudioSource to play the SFX
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                source.clip = sfxAudioClips[sfxClip];
                source.Play();
                return;
            }
        }

        // If all AudioSources are busy, create a new temporary one
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = sfxAudioClips[sfxClip];
        newSource.Play();
        Destroy(newSource, sfxAudioClips[sfxClip].length);
    }


    public void PlayUI()
    {
        if (PlayerPrefs.GetInt("isSound", 1) == 0)
            return;

        // Find an available AudioSource to play the SFX
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                source.clip = UIClip;
                source.Play();
                return;
            }
        }

        // If all AudioSources are busy, create a new temporary one
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = UIClip;
        newSource.Play();
        Destroy(newSource, UIClip.length);
    }
}
