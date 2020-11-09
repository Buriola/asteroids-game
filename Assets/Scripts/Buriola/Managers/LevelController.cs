using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Buriola.Audio;
using Buriola.Controller;
using Buriola.UI;
using UnityEngine.Serialization;

namespace Buriola.Managers
{
    public class LevelController : MonoBehaviour
    {
        #region Variables

        public static LevelController Instance;

        [FormerlySerializedAs("spawnPointsParent")] 
        [Header("Level Settings")]
        [SerializeField]
        private Transform _spawnPointsParent = null;
        
        [FormerlySerializedAs("player")] 
        [SerializeField]
        private StateManager _player = null;
        
        [FormerlySerializedAs("waves")] 
        [SerializeField]
        private List<Wave> _waves = new List<Wave>();
        
        public int PlayerScore { get; set; }
        public int HighestScore { get; private set; }
        public int EnemiesKilled { get; set; }

        [FormerlySerializedAs("levelMusic")] 
        [Header("Audio Settings")] 
        [SerializeField]
        private AudioClip _levelMusic = null;

        private int _waveIndex;
        private bool _waveComplete;
        private int _lastWaveIndex;
        private bool _isLastWave;

        #endregion

        #region Unity Functions

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();

            _waveIndex = 0;
            _waveComplete = false;

            LoadHighScore();

            if (_levelMusic != null)
                AudioHandler.PlayMusic(_levelMusic, true);
            
            if (_waves.Count > 0)
            {
                StartCoroutine(StartWave());
                _lastWaveIndex = _waves.Count - 1;
            }
            else
                Debug.Log("No waves set in the inspector, please verify.");
        }

        private void Update()
        {
            if (_player.IsDead)
            {
                StopAllCoroutines();
                
                if (PlayerScore > HighestScore)
                    SaveHighScore(PlayerScore);

                HUD.Instance.RetryMenu.gameObject.SetActive(true);
            }
            
            if (_waveComplete)
            {
                if (_waveIndex < _lastWaveIndex)
                {
                    _waveIndex++;
                    _waveComplete = false;
                    StartCoroutine(StartWave());
                    EnemiesKilled = 0;
                    return;
                }

                if (_waveIndex == _lastWaveIndex && !_isLastWave)
                {
                    _waveComplete = false;
                    _isLastWave = true;
                    StartCoroutine(StartWave());
                    EnemiesKilled = 0;
                    return;
                }

                if (_isLastWave)
                {
                    _isLastWave = false;
                    _waveIndex = 0;
                    _waveComplete = false;
                    StartCoroutine(StartWave());
                    EnemiesKilled = 0;

                    if (PlayerScore > HighestScore)
                        SaveHighScore(PlayerScore);
                }
            }
        }

        #endregion

        #region My Functions
        private IEnumerator StartWave()
        {

            yield return new WaitForSeconds(_waves[_waveIndex].StartTime);

            while (EnemiesKilled < _waves[_waveIndex].NumberOfEnemies)
            {
                yield return new WaitForSeconds(_waves[_waveIndex].SpawnRate);
                string tag = TakeOneEnemyFromWave();
                int r = RandomSpawnPoint();

                GameObject enemy = ObjectPooler.Instance.GetEnemyObject(tag);
                
                enemy.transform.position = _spawnPointsParent.GetChild(r).position;
                enemy.transform.rotation = _spawnPointsParent.GetChild(r).rotation;
                enemy.SetActive(true);
            }
            
            _waveComplete = true;
        }
        
        private string TakeOneEnemyFromWave()
        {
            string retVal = "";
            float rateSum = 0;

            for (int i = 0; i < _waves[_waveIndex].WaveEnemies.Count; i++)
            {
                float rate = _waves[_waveIndex].WaveEnemies[i].Rate;
                float r = Random.Range(0, rateSum + rate);
                if (r >= rateSum)
                    retVal = _waves[_waveIndex].WaveEnemies[i].EnemyTag;

                rateSum += _waves[_waveIndex].WaveEnemies[i].Rate;
            }

            return retVal;
        }
        
        private int RandomSpawnPoint()
        {
            int retVal = 0;

            int count = _spawnPointsParent.childCount;
            retVal = Random.Range(0, count - 1);

            return retVal;
        }
        
        private void SaveHighScore(int score)
        {
            HighestScore = score;
            
            PlayerPrefs.SetInt("HighScore" + SceneManager.GetActiveScene().buildIndex.ToString(), HighestScore);
        }
        
        private void LoadHighScore()
        {
            HighestScore = PlayerPrefs.GetInt("HighScore" + SceneManager.GetActiveScene().buildIndex);
        }

        #endregion
    }
}
