using System;

namespace LevelGenerator.Scripts
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

    internal static class Directions {
    // Item1 and Item2 must be { -1, 0, +1 } and Item1 != Item2
        private static Direction From(Tuple<int, int> delta) =>
                delta.Item1 != 0 ?
                    delta.Item1 == 1?  Direction.Right : Direction.Left :
                    delta.Item2 == 1?  Direction.Up : Direction.Down;

        internal static Direction From(Tuple<int, int> from, Tuple<int, int> to) =>
            From(new Tuple<int, int>(
                to.Item1 - from.Item1,
                to.Item2 - to.Item1
            ));
    }
}