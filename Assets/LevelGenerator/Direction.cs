using System;
using System.Collections.Generic;

namespace LevelGenerator
{
    internal enum Direction { Left, Up, Right, Down }

    internal static class DirectionExtensions
    {
        internal static Tuple<int, int> NextPosition(this Direction direction, int fromX, int fromY)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Tuple<int, int>(fromX - 1, fromY);
                case Direction.Up:
                    return new Tuple<int, int>(fromX, fromY + 1);
                case Direction.Right:
                    return new Tuple<int, int>(fromX + 1, fromY);
                case Direction.Down:
                    return new Tuple<int, int>(fromX, fromY - 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}