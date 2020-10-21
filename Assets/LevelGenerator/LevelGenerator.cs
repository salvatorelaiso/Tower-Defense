#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelGenerator
{
    internal static class LevelGenerator
    {
        private const string TemplateLevelPath = "Assets/LevelGenerator/LevelTemplate/LevelTemplate.unity";
        
        private const string LevelsPath = "Assets/Scenes/";
        private const string LevelString = "Level";
        private const string LevelExtension = ".unity";

        

        public static void CloneLevel(int levelIndex, int rows, int columns)
        {
            var suffix = $"{levelIndex:00}";
            var levelName = LevelString + suffix + LevelExtension;
            var dest = LevelsPath + levelName;
            
            var templateLevel = EditorSceneManager.OpenScene(TemplateLevelPath, OpenSceneMode.Single);
            if (EditorSceneManager.SaveScene(templateLevel, dest, true))
            {
                var newScene = EditorSceneManager.OpenScene(dest, OpenSceneMode.Single);
                Init(newScene, rows, columns);
            }
            else
            {
                Debug.Log("Could not create the scene!");
            }
        }

        private static void Init(Scene newScene, int rows, int columns)
        {
            var start = Resources.Load("LevelGenerator/Prefabs/START");
            var end = Resources.Load("LevelGenerator/Prefabs/END");
            var node = Resources.Load("LevelGenerator/Prefabs/Node");
            const float nodeDim = 5f;

            var nodes = GameObject.Find("Nodes");
            
            for (int i = 0; i < rows; i++)
            {
                var row = new GameObject($"Row ({i})");
                row.transform.parent = nodes.transform;
                for (int j = 0; j < columns; j++)
                {
                    // START position
                    if (i == j && j == 1)
                    {
                        var position = new Vector3(j * nodeDim, 2.5f, -i * nodeDim);
                        var obj = Object.Instantiate(start,
                            position: position,
                            rotation: Quaternion.identity) as GameObject;
                        obj.name = "START";
                        obj.transform.SetSiblingIndex(nodes.transform.GetSiblingIndex()+1);
                    }
                    
                    // END position
                    else if (i == rows-2 && j == columns-2)
                    {
                        var position = new Vector3(j * nodeDim, 2.5f, -i * nodeDim);
                        var obj = Object.Instantiate(end,
                            position: position,
                            rotation: Quaternion.identity) as GameObject;
                        obj.name = "END";
                        obj.transform.SetSiblingIndex(nodes.transform.GetSiblingIndex()+2);
                    }
                    
                    else
                    {
                        var position = new Vector3(j * nodeDim, 0, -i * nodeDim);
                        Object.Instantiate(node,
                                parent: row.transform,
                                position: position,
                                rotation: Quaternion.identity)
                            .name = $"Node ({j})";
                    }
                }
            }
        }
    }
}
#endif