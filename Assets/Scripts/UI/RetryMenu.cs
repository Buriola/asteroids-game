using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class to handle the retry menu
/// </summary>
public class RetryMenu : AutoReferencer<RetryMenu>
{
    #region Variables
    public Button yesButton;
    public Button exitButton;
    #endregion

    private void Awake()
    {
        yesButton.onClick.AddListener(YesButton);
        exitButton.onClick.AddListener(ExitButton);
    }

    /// <summary>
    /// Yes Button action
    /// </summary>
    private void YesButton()
    {
        SceneLoader.ReloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    
    /// <summary>
    /// Exit button action
    /// </summary>
    private void ExitButton()
    {
        SceneLoader.LoadLevel(0);
    }
	
}
