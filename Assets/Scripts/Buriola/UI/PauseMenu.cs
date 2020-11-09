using System;
using Buriola.Audio;
using Buriola.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Buriola.UI
{
    public class PauseMenu : AutoReferencer<PauseMenu>
    {
        #region Variables

        [FormerlySerializedAs("resumeButton")] 
        [SerializeField]
        private Button _resumeButton = null;
        
        [FormerlySerializedAs("mainMenuButton")] 
        [SerializeField]
        private Button _mainMenuButton = null;
        public bool Paused { get; set; }

        #endregion

        private void Awake()
        {
            _resumeButton.onClick.AddListener(ResumeButton);
            _mainMenuButton.onClick.AddListener(MainMenuButton);
        }
        
        private void ResumeButton()
        {
            Paused = false;
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        
        private void MainMenuButton()
        {
            AudioHandler.StopAllSounds();
            Time.timeScale = 1;
            SceneLoader.LoadLevel(0);
        }

        private void OnDestroy()
        {
            _resumeButton.onClick.RemoveListener(ResumeButton);
            _mainMenuButton.onClick.RemoveListener(MainMenuButton);
        }
    }
}
