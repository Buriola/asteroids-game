using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle the main menu and the buttons 
/// </summary>
public class MainMenu : AutoReferencer<MainMenu>
{
    #region Variables
    [Header("UI Settings")]
    public GameObject selectMenu;
    public Button startButton;
    public Button exitButton;

    [Header("Audio Settings")]
    public AudioClip music;
    #endregion  

    void Start ()
    {
        startButton.onClick.AddListener(StartButton); // Adds a listener
        exitButton.onClick.AddListener(ExitButton);

        if (music != null)
            AudioHandler.PlayMusic(music, true); // plays the music for the main menu
	}

    #region Variables

    /// <summary>
    /// Start Button action
    /// </summary>
    private void StartButton()
    {
        selectMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Exit button action
    /// </summary>
    private void ExitButton()
    {
        Application.Quit();
    }

    #endregion
}
