using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asteroids.Controller;

/// <summary>
/// Class to control and update the HUD elements
/// </summary>
public class HUD : MonoBehaviour
{
    #region Variables
    public static HUD instance;

    public Image playerSprite;
    public Text healthText;
    public Slider xpBar;
    public Text levelText;
    public Text highScoreText;
    public Text scoreText;
    
    public LevelController level;
    public PauseMenu pauseMenu;
    public RetryMenu retryMenu;

    private StateManager st;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        st = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
        pauseMenu.gameObject.SetActive(false);
        retryMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateHUD();
        Pause();
    }
    #endregion

    #region My Functions

    /// <summary>
    /// Updates the HUD on the screen
    /// </summary>
    private void UpdateHUD()
    {
        playerSprite.sprite = st.gameObject.GetComponent<SpriteRenderer>().sprite;
        healthText.text = "x " + st.stats.health.ToString();
        xpBar.maxValue = st.stats.maxXP;
        xpBar.value = st.stats.xp;
        levelText.text = st.stats.level.ToString();
        highScoreText.text = level.highestScore.ToString();
        scoreText.text = "" + level.playerScore.ToString("00000000");
    }

    /// <summary>
    /// Shows the pause menu
    /// </summary>
    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.paused)
            {
                pauseMenu.paused = true;
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.paused = false;
                pauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
            }

        }
    }
    #endregion
}
