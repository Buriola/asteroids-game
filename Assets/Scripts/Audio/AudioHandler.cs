using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class serves as an universal AudioHandler to play sfx and music from anywhere in the code.
/// </summary>
public class AudioHandler : MonoBehaviour
{
    #region Variables
    private static AudioSource musicSource;
    private static AudioSource sfxSource;
    private static AudioHandler instance;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null || instance != this)
        {
            Destroy(gameObject);
            return;
        }
           
        DontDestroyOnLoad(gameObject);
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        sfxSource = transform.GetChild(1).GetComponent<AudioSource>();
    }

    #region My Functions

    /// <summary>
    /// Play a sound effect once and gives it a random pitch
    /// </summary>
    /// <param name="clip">Sfx to play</param>
    public static void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(0.8f, 1.2f);
        sfxSource.PlayOneShot(clip);
    }
    
    /// <summary>
    /// Play a background music. You can choose wheter you want it to loop or not
    /// </summary>
    /// <param name="clip">The music you want to play</param>
    /// <param name="loop">Loop this?</param>
    public static void PlayMusic(AudioClip clip, bool loop = false)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    /// <summary>
    /// Stop all audio sources from playing. Use this to transit between scenes
    /// </summary>
    public static void StopAllSounds()
    {
        musicSource.Stop();
        sfxSource.Stop();
    }
    #endregion
}
