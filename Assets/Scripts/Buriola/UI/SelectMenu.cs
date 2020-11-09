using Buriola.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Buriola.UI
{
    public class SelectMenu : MonoBehaviour
    {
        #region Variables

        [FormerlySerializedAs("initMenu")] 
        [SerializeField]
        private GameObject _initMenu = null;
        
        [FormerlySerializedAs("level1Button")] 
        [SerializeField]
        private Button _level1Button = null;
        
        [FormerlySerializedAs("level2Button")] 
        [SerializeField]
        private Button _level2Button = null;
        
        [FormerlySerializedAs("backButton")]
        [SerializeField]
        private Button _backButton = null;

        [FormerlySerializedAs("highscoreText1")] 
        [SerializeField]
        private Text _highscoreText1 = null;
        
        [FormerlySerializedAs("highscoreText2")] 
        [SerializeField]
        private Text _highscoreText2 = null;

        #endregion

        private void OnEnable()
        {
            _highscoreText1.text = "Highest Score:\n" + PlayerPrefs.GetInt("HighScore1");
            _highscoreText2.text = "Highest Score:\n" + PlayerPrefs.GetInt("HighScore2");

            _level1Button.onClick.AddListener(LoadLevel_1);
            _level2Button.onClick.AddListener(LoadLevel_2);
            _backButton.onClick.AddListener(BackButton);
        }
        
        private void LoadLevel_1()
        {
            SceneLoader.LoadLevel(1);
        }
        
        private void LoadLevel_2()
        {
            SceneLoader.LoadLevel(2);
        }
        
        private void BackButton()
        {
            _initMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
