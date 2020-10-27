using UnityEngine;

namespace LevelGenerator.Scripts
{
    internal static class ResourceManager
    {
        internal static readonly Object Start = Resources.Load("LevelGenerator/Prefabs/START");
        internal static readonly Object End = Resources.Load("LevelGenerator/Prefabs/END");
        internal static readonly Object Node = Resources.Load("LevelGenerator/Prefabs/Node");
        internal static readonly Object Ground = Resources.Load("LevelGenerator/Prefabs/Ground");
        internal static readonly Object Connector = Resources.Load("LevelGenerator/Prefabs/Connector");
    }
}