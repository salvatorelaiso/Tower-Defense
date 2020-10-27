using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelGenerator.Scripts
{
    internal static class WaypointGenerator
    {
        public static IEnumerable<Tuple<int, int>> GenerateWaypoints(Level level)
        {
            var waypoints = new List<Tuple<int, int>>();

            var previousPosition = level.PathIterator().First();
            var actualPosition = level.PathIterator().Skip(1).First();
            var actualDirection = Directions.From(previousPosition, actualPosition);

            foreach (var path in level.PathIterator().Skip(2))
            {
                previousPosition = actualPosition;
                actualPosition = path;
                var previousDirection = actualDirection;
                actualDirection = Directions.From(previousPosition, actualPosition);
                if (previousDirection != actualDirection)
                {
                    waypoints.Add(previousPosition);
                }
            }
            waypoints.Add(actualPosition);
            return waypoints;
        }
        
    }
}