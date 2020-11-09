using Buriola.Audio;
using Buriola.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Buriola.UI
{
    public class MainMenu : AutoReferencer<MainMenu>
    {
        #region Variables

        [FormerlySerializedAs("selectMenu")]
        [Header("UI Settings")] 
        [SerializeField]
        private GameObject _selectMenu = null;
        
        [FormerlySerializedAs("startButton")] 
        [SerializeField]
        private Button _startButton = null;
        
        [FormerlySerializedAs("exitButton")] 
        [SerializeField]
        private Button _exitButton = null;

        [FormerlySerializedAs("music")]
        [Header("Audio Settings")]
        [SerializeField]
        private AudioClip _music = null;

        #endregion

        void Start()
        {
            _startButton.onClick.AddListener(StartButton); // Adds a listener
            _exitButton.onClick.AddListener(ExitButton);

            if (_music != null)
                AudioHandler.PlayMusic(_music, true); // plays the music for the main menu
        }

        #region Variables
        private void StartButton()
        {
            _selectMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        
        private void ExitButton()
        {
            Application.Quit();
        }

        #endregion
    }
}
