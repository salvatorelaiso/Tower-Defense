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

        public static IEnumerable<Tuple<int, int>> PathIterator(this Level level)
        {
            var paths = level.Paths;
            var previousX = -1;
            var previousY = -1;
            var x = 1;
            var y = 1;
            yield return new Tuple<int, int>(x, y);

            var reachedEnd = false;
            while (!reachedEnd)
            {
                var cannotFindPath = true;
                foreach (var direction in (Direction[]) Enum.GetValues(typeof(Direction)))
                {
                    var (newX, newY) = direction.NextPosition(x, y);
                    if (paths[newX, newY] && (newX != previousX || newY != previousY))
                    {
                        cannotFindPath = false;
                        yield return new Tuple<int, int>(newX, newY);
                        (previousX, previousY) = (x, y);
                        (x, y) = (newX, newY);
                        reachedEnd = x == level.EndPositionX && y == level.EndPositionY;
                        break;
                    }
                }
                if (cannotFindPath) throw new Exception("Cannot find Path!");
            }
        }
    }
}