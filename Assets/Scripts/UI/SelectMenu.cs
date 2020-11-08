using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle the select level menu
/// </summary>
public class SelectMenu : MonoBehaviour
{
    #region Variables
    public GameObject initMenu;
    public Button level1Button;
    public Button level2Button;
    public Button backButton;

    public Text highscoreText1;
    public Text highscoreText2;

    #endregion

    private void OnEnable()
    {
        highscoreText1.text = "Highest Score:\n" + PlayerPrefs.GetInt("HighScore1"); //Gets the highscores
        highscoreText2.text = "Highest Score:\n" + PlayerPrefs.GetInt("HighScore2");

        level1Button.onClick.AddListener(LoadLevel_1);
        level2Button.onClick.AddListener(LoadLevel_2);
        backButton.onClick.AddListener(BackButton);
    }

    /// <summary>
    /// Level 1 button action
    /// </summary>
    private void LoadLevel_1()
    {
        SceneLoader.LoadLevel(1);
    }
    
    /// <summary>
    /// Level 2 button action
    /// </summary>
    private void LoadLevel_2()
    {
        SceneLoader.LoadLevel(2);
    }

    /// <summary>
    /// Back button action
    /// </summary>
    private void BackButton()
    {
        initMenu.SetActive(true);
        gameObject.SetActive(false);
    }
 }
