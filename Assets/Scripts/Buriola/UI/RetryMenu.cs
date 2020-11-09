using Buriola.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Buriola.UI
{
    public class RetryMenu : AutoReferencer<RetryMenu>
    {
        #region Variables

        [FormerlySerializedAs("yesButton")] 
        [SerializeField]
        private Button _yesButton = null;

        [FormerlySerializedAs("exitButton")]
        [SerializeField] 
        private Button _exitButton = null;

        #endregion

        private void Awake()
        {
            _yesButton.onClick.AddListener(YesButton);
            _exitButton.onClick.AddListener(ExitButton);
        }
        
        private void YesButton()
        {
            SceneLoader.ReloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        
        private void ExitButton()
        {
            SceneLoader.LoadLevel(0);
        }

        private void OnDestroy()
        {
            _yesButton.onClick.RemoveListener(YesButton);
            _exitButton.onClick.RemoveListener(ExitButton);
        }
    }
}
