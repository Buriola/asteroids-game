using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buriola.Utilities
{
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// Loads a level async and additive
        /// </summary>
        /// <param name="levelID">Id of the level</param>
        public static void LoadLevelAsyncAdditive(int levelID)
        {
            if (!SceneManager.GetSceneByBuildIndex(levelID).isLoaded)
            {
                SceneManager.LoadSceneAsync(levelID, LoadSceneMode.Additive);
            }
        }

        /// <summary>
        /// Loads a level
        /// </summary>
        /// <param name="levelID"></param>
        public static void LoadLevel(int levelID)
        {
            SceneManager.LoadScene(levelID);
        }

        /// <summary>
        /// Loads a level async and additive
        /// </summary>
        /// <param name="levelName">Name of the level</param>
        public static void LoadLevelAsyncAdditive(string levelName)
        {
            if (!SceneManager.GetSceneByName(levelName).isLoaded)
            {
                SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            }
        }

        /// <summary>
        /// Unloads a level async
        /// </summary>
        /// <param name="levelID">ID of the level</param>
        public static void UnloadLevelAsync(int levelID)
        {
            if (SceneManager.GetSceneByBuildIndex(levelID).isLoaded)
            {
                SceneManager.UnloadSceneAsync(levelID);
            }
        }

        /// <summary>
        /// Unloads a level async
        /// </summary>
        /// <param name="levelName">Name of the level</param>
        public static void UnloadLevelAsync(string levelName)
        {
            if (SceneManager.GetSceneByName(levelName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(levelName);
            }
        }

        /// <summary>
        /// Reloads a level
        /// </summary>
        /// <param name="levelID">Id of the level</param>
        public static void ReloadSceneAsync(int levelID)
        {
            if (SceneManager.GetSceneByBuildIndex(levelID).isLoaded)
            {
                LoadLevel(levelID);
            }
        }
    }
}
