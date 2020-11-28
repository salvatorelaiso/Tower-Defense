using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace LevelGenerator.Scripts
{
    internal static class Utils
    {
        private static readonly Random Random = new Random();

        internal static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        internal static bool IsInsideBoundaries(int x, int y, int xBound, int yBound) =>
            x >= 0 && x < xBound && y >= 0 && y < yBound;

        internal static Object RelatedResource(CellContent content)
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

        private static IEnumerable<Tuple<int, int>> AdjacentsOf(int x, int y, int width, int height)
        {
            // Left column
            var leftColumn = x - 1;
            if (leftColumn > 0)
            {
                for (int deltaY = -1; deltaY < 2; deltaY++)
                {
                    if (IsInsideBoundaries(leftColumn, y + deltaY, width, height))
                    {
                        yield return new Tuple<int, int>(leftColumn, y + deltaY);
                    }
                }
            }

            if (IsInsideBoundaries(x, y - 1, width, height))
            {
                yield return new Tuple<int, int>(x, y - 1);
            }
            
            if (IsInsideBoundaries(x, y + 1, width, height))
            {
                yield return new Tuple<int, int>(x, y + 1);
            }
            
            // Right column
            var rightColumn = x + 1;
            if (rightColumn < width)
            {
                for (int deltaY = -1; deltaY < 2; deltaY++)
                {
                    if (IsInsideBoundaries(rightColumn, y + deltaY, width, height))
                    {
                        yield return new Tuple<int, int>(rightColumn, y + deltaY);
                    }
                }
            }
        }

        internal static IEnumerable<Tuple<int, int, int, int>> Adjacents(Level level)
        {
            for (int x = 0; x < level.Width; x++)
            {
                for (int y = 0; y < level.Height; y++)
                {
                    foreach (var adjacent in AdjacentsOf(x, y, level.Width, level.Height))
                    {
                        yield return new Tuple<int, int, int, int>(x, y, adjacent.Item1, adjacent.Item2);
                    }
                }
            }
        }
        
        
    }
}
