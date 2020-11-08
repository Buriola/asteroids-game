using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle the pause menu
/// </summary>
public class PauseMenu : AutoReferencer<PauseMenu>
{
    #region Variables
    public Button resumeButton;
    public Button mainMenuButton;
    public bool paused;
    #endregion

    private void Awake()
    {
        resumeButton.onClick.AddListener(ResumeButton);
        mainMenuButton.onClick.AddListener(MainMenuButton);
    }

    /// <summary>
    /// Resume button action
    /// </summary>
    private void ResumeButton()
    {
        paused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Main menu button action
    /// </summary>
    private void MainMenuButton()
    {
        AudioHandler.StopAllSounds();
        Time.timeScale = 1;
        SceneLoader.LoadLevel(0);
    }
}
