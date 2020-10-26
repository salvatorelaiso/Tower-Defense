using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGenerator
{
    internal static class WaypointGenerator
    {
        public static List<Tuple<int, int>> GenerateWaypoints(Level level)
        {
            var endX = level.EndPositionX;
            var endY = level.EndPositionY;
            var paths = LevelToBool(level);
            var waypoints = new List<Tuple<int, int>>();

            var previousX = 1;
            var previousY = 1;
            
            var previousDirection = paths[previousX + 1, previousY] ? Direction.Right : Direction.Up;
            
            var (actualX, actualY) = previousDirection.NextPosition(previousX, previousY);

            bool reachedEnd = false;
            while (!reachedEnd)
            {
                Direction tempDirection;
                int tempX;
                int tempY;
                (tempDirection, tempX, tempY) = FindNextPosition(paths, previousX, previousY, actualX, actualY);
                
                if (previousDirection != tempDirection)
                {
                    previousDirection = tempDirection;
                    waypoints.Add(new Tuple<int, int>(actualX, actualY));
                }
                                
                (previousX, previousY) = (actualX, actualY);
                (actualX, actualY) = (tempX, tempY);

                if (actualX == endX && actualY == endY)
                {
                    reachedEnd = true;
                    waypoints.Add(new Tuple<int, int>(actualX, actualY));
                }
            }
            return waypoints;
        }

        private static bool[,] LevelToBool(Level level)
        {
            var paths = new bool[level.Width, level.Height];
            for (int x = 0; x < level.Width; x++)
            {
                for (int y = 0; y < level.Height; y++)
                {
                    paths[x, y] = level.GetCellContent(x, y) != CellContent.Node;
                }
            }
            return paths;
        }

        private static Tuple<Direction, int, int> FindNextPosition(bool[,] paths, int previousX, int previousY, int x, int y)
        {
            var s = "\n";
            for (int i = paths.GetLength(1)-1; i >= 0; i--)
            {
                for (int j = 0; j < paths.GetLength(0); j++)
                {
                    s += paths[j, i] ? "\t1\t" : "\t0\t";
                }

                s += "\n\n";
            }

            Debug.Log(s);
            
            foreach (var direction in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                var (newX, newY) = direction.NextPosition(x, y);
                if (paths[newX, newY] && (newX != previousX || newY != previousY))
                {
                    return new Tuple<Direction, int, int>(direction, newX, newY);
                }
            }
            throw new Exception("Cannot find path!");
        }
    }
}