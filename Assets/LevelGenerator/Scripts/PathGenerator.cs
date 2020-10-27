using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelGenerator.Scripts
{
    internal static class PathGenerator
    {
        private const int EdgeDimension = 1;
        private enum CellStatus { Usable, Unusable }

        public static Level Generate(Level level, bool random = false)
        {
            var statuses = InitStatus(level);
            return FindPath(level, statuses, Level.StartPositionX, Level.StartPositionY, random);
        }

        private static CellStatus[,] InitStatus(Level level)
        {
            var statuses = new CellStatus[level.Width, level.Height];
            statuses[Level.StartPositionX, Level.StartPositionY] = CellStatus.Unusable;
            statuses[level.EndPositionX, level.EndPositionY] = CellStatus.Unusable;
            for (int y = 0; y < level.Height; y++)
            {
                statuses[0, y] = CellStatus.Unusable;
                statuses[level.Width - EdgeDimension, y] = CellStatus.Unusable;
            }
            for (int x = 0; x < level.Width; x++)
            {
                statuses[x, 0] = CellStatus.Unusable;
                statuses[x, level.Height - EdgeDimension] = CellStatus.Unusable;
            }
            return statuses;
        }

        private static Level FindPath(Level level, CellStatus[,] statuses, int x, int y, bool random)
        {
            if (IsConnected(x, y, level.EndPositionX, level.EndPositionY)) return level;

            var availableDirections = GetAvailableDirections(level, statuses, x, y);
            if (ReachedDeadEnd(availableDirections)) return null;

            if (random) availableDirections.Shuffle();
            
            foreach (var direction in availableDirections)
            {
                // Make clones in order to explore this branch without side effects on the original Objects
                var clonedLevel = (Level)level.Clone();
                var clonedStatuses = new CellStatus[level.Width, level.Height];
                Array.Copy(statuses, clonedStatuses, statuses.Length);

                // Set the destination Cell as Path
                var (destinationX, destinationY) = direction.NextPosition(x, y);
                clonedLevel.SetPathAt(destinationX, destinationY);
                clonedStatuses[destinationX, destinationY] = CellStatus.Unusable;
      
                // Set the other destinations as unusable
                foreach (var otherDirection in availableDirections.Where(dir => dir != direction))
                {
                    var (unusableX, unusableY) = otherDirection.NextPosition(x, y);
                    clonedStatuses[unusableX, unusableY] = CellStatus.Unusable;
                }
                
                // Recursive call
                var path = FindPath(clonedLevel, clonedStatuses, destinationX, destinationY, random);
                // If path is not null means we have reached the end
                if (path != null) return path;
            }

            return null;
        }

        private static List<Direction> GetAvailableDirections(Level level, CellStatus[,] statuses, int originX, int originY)
        {
            var directions = new List<Direction>();
            foreach (var direction in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                var (destinationX, destinationY) = direction.NextPosition(originX, originY);
                
                if ( 
                    Utils.IsInsideBoundaries(destinationX, destinationY, level.Width, level.Height) &&
                    statuses[destinationX, destinationY] == CellStatus.Usable)
                {
                    directions.Add(direction);
                }
            }
            return directions;
        }
        
        private static bool IsConnected(int x1, int y1, int x2, int y2) =>
            Math.Abs(x1 - x2) + Math.Abs(y1 - y2) == 1;

        private static bool ReachedDeadEnd(IEnumerable<Direction> availableDirections) =>
            !availableDirections.Any();

    }
}