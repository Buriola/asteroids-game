using UnityEngine;
using UnityEngine.UI;
using Buriola.Controller;
using Buriola.Managers;
using UnityEngine.Serialization;

namespace Buriola.UI
{
    public class HUD : MonoBehaviour
    {
        #region Variables

        public static HUD Instance;

        [FormerlySerializedAs("playerSprite")] 
        [SerializeField]
        private Image _playerSprite = null;
        
        [FormerlySerializedAs("healthText")]
        [SerializeField]
        private Text _healthText = null;
        
        [FormerlySerializedAs("xpBar")] 
        [SerializeField]
        private Slider _xpBar = null;
        
        [FormerlySerializedAs("levelText")] 
        [SerializeField]
        private Text LevelText = null;
        
        [FormerlySerializedAs("highScoreText")] 
        [SerializeField]
        private Text _highScoreText = null;
        
        [FormerlySerializedAs("scoreText")]
        [SerializeField]
        private Text _scoreText = null;

        [FormerlySerializedAs("level")] 
        [SerializeField]
        private LevelController _levelController = null;
        
        [FormerlySerializedAs("pauseMenu")] 
        [SerializeField]
        private PauseMenu _pauseMenu = null;
        
        [FormerlySerializedAs("retryMenu")]
        public RetryMenu RetryMenu = null;

        private StateManager _stateManager;

        #endregion

        #region Unity Functions

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _stateManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
            _pauseMenu.gameObject.SetActive(false);
            RetryMenu.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateHUD();
            Pause();
        }

        #endregion

        #region My Functions
        
        private void UpdateHUD()
        {
            _playerSprite.sprite = _stateManager.gameObject.GetComponent<SpriteRenderer>().sprite;
            _healthText.text = "x " + _stateManager.Stats.Health.ToString();
            _xpBar.maxValue = _stateManager.Stats.MaxXP;
            _xpBar.value = _stateManager.Stats.Xp;
            LevelText.text = _stateManager.Stats.Level.ToString();
            _highScoreText.text = _levelController.HighestScore.ToString();
            _scoreText.text = "" + _levelController.PlayerScore.ToString("00000000");
        }
        
        private void Pause()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_pauseMenu.Paused)
                {
                    _pauseMenu.Paused = true;
                    _pauseMenu.gameObject.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    _pauseMenu.Paused = false;
                    _pauseMenu.gameObject.SetActive(false);
                    Time.timeScale = 1;
                }

            }
        }

        #endregion
    }
}
