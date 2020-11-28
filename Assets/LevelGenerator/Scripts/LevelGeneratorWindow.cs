using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class LevelGeneratorWindow : EditorWindow
    {
        private const string LevelsPath = "Assets/Scenes";
        private const int LevelNumberCharacters = 2;
        
        private int levelNumber = 0;
        
        private int rows;
        private const int MinRows =   3;
        private const int MaxRows =  30;
        private int columns;
        private const int MinColumns =   4;
        private const int MaxColumns =  30;

        private bool random;

        // Add menu named "Generator" to the Window menu
        [MenuItem("Window/Generator")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            var window = (LevelGeneratorWindow) GetWindow(typeof(LevelGeneratorWindow));
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label($"Level {levelNumber:00}", EditorStyles.boldLabel);
            rows = EditorGUILayout.IntSlider("Rows", rows, MinRows, MaxRows);
            columns = EditorGUILayout.IntSlider("Columns", columns, MinColumns, MaxColumns);
            random = EditorGUILayout.Toggle("Random Path", random);
            EditorGUILayout.HelpBox("Random Path could cause problems (i.e. Unity freezes).", MessageType.Warning);
            if (GUILayout.Button("Generate"))
            {
                EditorSceneManager.SaveOpenScenes();
                LevelGenerator.CloneLevel(levelNumber, rows, columns, random);
            }
        }

        private void OnInspectorUpdate()
        {
            levelNumber = NextLevelIndex();
        }

        private static int NextLevelIndex()
        {
            // Check the highest Level already created
            var lastLevel =
                Directory.EnumerateFiles(LevelsPath, "Level??.unity").Last();
            var extensionDotIndex =
                lastLevel.LastIndexOf('.');
            var number =
                int.Parse(lastLevel.Substring(extensionDotIndex - LevelNumberCharacters, LevelNumberCharacters));
            // Return the next int
            return ++number;
        }
    }
}
