#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelGenerator
{
    internal static class LevelGenerator
    {
        private const string TemplateLevelPath = "Assets/LevelGenerator/LevelTemplate/LevelTemplate.unity";
        private const string TemplateLevelName = "LevelTemplate";
        
        private const string LevelsPath = "Assets/Scenes/";
        private const string LevelString = "Level";
        private const string LevelExtension = ".unity";

        public static void CloneLevel(int levelIndex)
        {
            var suffix = $"{levelIndex:00}";
            var levelName = LevelString + suffix + LevelExtension;
            var dest = LevelsPath + levelName;
            var scene = EditorSceneManager.OpenScene(TemplateLevelPath, OpenSceneMode.Single);
            if (EditorSceneManager.SaveScene(scene, dest, true))
            {
                EditorSceneManager.OpenScene(dest, OpenSceneMode.Single);
            }
            else
            {
                Debug.Log("Could not create the scene!");
            }
        }
    }
}
#endif