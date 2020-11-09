using UnityEditor;
using UnityEditor.SceneManagement;

namespace Buriola.Editor
{
    public class SceneItem : UnityEditor.Editor
    {
        [MenuItem("Open Scene/MainMenu")]
        public static void OpenPlayground()
        {
            OpenScene("MainMenu");
        }

        [MenuItem("Open Scene/Level1")]
        public static void OpenLevel1()
        {
            OpenScene("Level1");
        }

        [MenuItem("Open Scene/Level2")]
        public static void OpenLevel2()
        {
            OpenScene("Level2");
        }

        private static void OpenScene(string name)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }
}
