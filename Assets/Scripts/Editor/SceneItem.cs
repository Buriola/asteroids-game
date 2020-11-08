using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace EditorUtilities
{
    public class SceneItem : Editor
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

        static void OpenScene(string name)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
            }
        }

    }
}
