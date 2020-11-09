using UnityEngine;

namespace Buriola.Audio
{
    public class AudioHandler : MonoBehaviour
    {
        #region Variables

        private static AudioSource _musicSource;
        private static AudioSource _sfxSource;
        private static AudioHandler _instance;

        #endregion

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != null || _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            _musicSource = transform.GetChild(0).GetComponent<AudioSource>();
            _sfxSource = transform.GetChild(1).GetComponent<AudioSource>();
        }

        #region My Functions
        
        public static void PlaySFX(AudioClip clip)
        {
            _sfxSource.clip = clip;
            _sfxSource.pitch = Random.Range(0.8f, 1.2f);
            _sfxSource.PlayOneShot(clip);
        }
        
        public static void PlayMusic(AudioClip clip, bool loop = false)
        {
            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }
        
        public static void StopAllSounds()
        {
            _musicSource.Stop();
            _sfxSource.Stop();
        }

        #endregion
    }
}
