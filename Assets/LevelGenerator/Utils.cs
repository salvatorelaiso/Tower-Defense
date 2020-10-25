using System;
using System.Collections.Generic;

namespace LevelGenerator
{
    internal static class Utils
    {
        private static readonly Random Random = new Random();
        
        internal static void Shuffle<T>(this IList<T> list)  
        {
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = Random.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
        
        internal static bool IsInsideBoundaries(int x, int y, int xBound, int yBound) =>
            x >= 0 && x < xBound && y >= 0 && y < yBound;

        internal static UnityEngine.Object RelatedResource(CellContent content)
        {
            switch (content)
            {
                case CellContent.Node:
                    return ResourceManager.Node;
                case CellContent.Path:
                    return ResourceManager.Ground;
                case CellContent.Start:
                    return ResourceManager.Start;
                case CellContent.End:
                    return ResourceManager.End;
                default:
                    throw new ArgumentOutOfRangeException(nameof(content), content, null);
            }
        }

    }
}