#if UNITY_EDITOR
using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LevelGenerator
{
    internal static class LevelGenerator
    {
        private const string TemplateLevelPath = "Assets/LevelGenerator/LevelTemplate/LevelTemplate.unity";

        private const string LevelsPath = "Assets/Scenes/";
        private const string LevelString = "Level";
        private const string LevelExtension = ".unity";

        private const float NodeDimension = 5f;

        public static void CloneLevel(int levelIndex, int rows, int columns, bool random)
        {
            var suffix = $"{levelIndex:00}";
            var levelName = LevelString + suffix + LevelExtension;
            var dest = LevelsPath + levelName;

            var templateLevel = EditorSceneManager.OpenScene(TemplateLevelPath, OpenSceneMode.Single);
            if (EditorSceneManager.SaveScene(templateLevel, dest, true))
            {
                var newScene = EditorSceneManager.OpenScene(dest, OpenSceneMode.Single);
                Init(newScene, rows, columns, random);
                EditorSceneManager.MarkSceneDirty(newScene);
                EditorSceneManager.SaveScene(newScene);
            }
            else
            {
                Debug.Log("Could not create the scene!");
            }
        }

        private static void Init(Scene newScene, int rows, int columns, bool random)
        {
            var (width, height) = (columns, rows);
            var level = new Level(width, height);
            level.GeneratePath(random);

            //bool[,] paths = PathGenerator.GeneratePath(level);
            //bool[,] waypoints = WaypointGenerator.GenerateWaypoints(paths, rows, columns);

            var gameMaster = GameObject.Find("GameMaster");
            var nodes = GameObject.Find("Nodes");
            var environment = GameObject.Find("Environment");

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                // Create Empty GameObject under Nodes
                var row = new GameObject($"Row ({rowIndex})");
                row.transform.parent = nodes.transform;

                for (int columnIndex = 0; columnIndex < columns; columnIndex++)
                {
                    var content = level.GetCell(columnIndex, rowIndex).Content;
                    
                    switch (content)
                    {
                        case CellContent.Node:
                            Generate(content, rowIndex, columnIndex);
                            break;
                        case CellContent.Path:
                            Generate(content, rowIndex, columnIndex);
                            break;
                        case CellContent.Start:
                            var start = (GameObject) Generate(content, rowIndex, columnIndex);
                            Generate(CellContent.Path, rowIndex, columnIndex);
                            // Fix unassigned reference in GameMaster
                            gameMaster.GetComponent<WaveSpawner>().spawnPoint = start.transform;
                            break;
                        case CellContent.End:
                            Generate(content, rowIndex, columnIndex);
                            Generate(CellContent.Path, rowIndex, columnIndex);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // Fix BottomCanvas Position
                var bottomCanvas = GameObject.Find("BottomCanvas");
                var bottomCanvasTransformPosition = bottomCanvas.transform.position;
                bottomCanvasTransformPosition.z = -(rows + 1) * NodeDimension;
                bottomCanvas.transform.position = bottomCanvasTransformPosition;
                
            }
        }

        private static Object Generate(CellContent content, int row, int column)
        {
            var y = content == CellContent.Start || content == CellContent.End ? 2.5f : 0f;
            var position = new Vector3(column * NodeDimension, y, -row * NodeDimension);
            return Object.Instantiate(Utils.RelatedResource(content),
                position: position,
                rotation: Quaternion.identity);
        }
        
    }
}
#endif