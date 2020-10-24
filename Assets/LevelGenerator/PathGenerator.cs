using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace LevelGenerator
{
    internal enum Cell
    {
        Empty, Path, Unusable 
    }
    
    internal static class PathGenerator
    {
        private enum Direction
        {
            Left, Up, Right, Down
        }
        
        private const int EdgeDimension = 1;
        
        internal static bool[,] GeneratePath(int rows, int columns)
        {
            var internalRows = rows - EdgeDimension * 2;
            var internalColumns = columns - EdgeDimension * 2;
            var matrix = new Cell[internalRows, internalColumns];
            
            // START
            matrix[0, 0] = Cell.Path;
            // END
            matrix[internalRows - 1, internalColumns - 1] = Cell.Path;
            
            var path = Dig(matrix, internalRows, internalColumns, 0, 0);
            var isPath = new bool[rows, columns];

            for (int i = 0; i < internalRows; i++)
            {
                for (int j = 0; j < internalColumns; j++)
                {
                    isPath[i + 1, j + 1] = path[i, j] == Cell.Path;
                }
            }
            
            return isPath;
        }

        private static Cell[,] Dig(Cell[,] matrix, int rows, int columns, int row, int col)
        {
            // Iterate over all possibilities
            var directions = GetAvailableDirections(matrix, rows, columns, row, col);
            
            Shuffle(directions);
            
            foreach (var direction in directions)
            {
                // Copy the matrix to explore this branch 
                var newMatrix = new Cell[rows, columns];
                Array.Copy(matrix, newMatrix, matrix.Length);

                // Set the destination as path
                var (rowDestination, colDestination) = GetDestination(row, col, direction);
                newMatrix[rowDestination, colDestination] = Cell.Path;
                
                // Set the other destinations as unusable
                foreach (var otherDirection in directions.Where(dir => dir != direction))
                {
                    var (xUnusable, yUnusable) = GetDestination(row, col, otherDirection);
                    newMatrix[xUnusable, yUnusable] = Cell.Unusable;
                }

                if (IsPathConnected(rowDestination, colDestination, rows-1, columns-1))
                {
                    return newMatrix;
                }
                var path = Dig(newMatrix, rows, columns, rowDestination, colDestination);
                if (path != null) return path;
            }
            return null;
        }

        private static bool IsPathConnected(int row, int col, int rowDestination, int colDestination) =>
            Math.Abs(row - rowDestination) + Math.Abs(col - colDestination) == 1;

        private static List<Direction> GetAvailableDirections(Cell[,] matrix, int rows, int columns, int row, int col)
        {
            var directions = new List<Direction>();
            if (row - 1 >= 0)
            {
                if (matrix[row - 1, col] == Cell.Empty)
                {
                    directions.Add(Direction.Left);
                }
            }
            if (row + 1 < rows)
            {
                if (matrix[row + 1, col] == Cell.Empty)
                {
                    directions.Add(Direction.Right);
                }        
            }
            if (col - 1 >= 0)
            {
                if (matrix[row, col - 1] == Cell.Empty)
                {
                    directions.Add(Direction.Down);
                }            
            }
            if (col + 1 < columns)
            {
                if (matrix[row, col + 1] == Cell.Empty)
                {
                    directions.Add(Direction.Up);
                }            
            }
            return directions;
        }

        private static Tuple<int, int> GetDestination(Tuple<int, int> actualPosition, Direction direction) =>
            GetDestination(actualPosition.Item1, actualPosition.Item2, direction);
        
        private static Tuple<int, int> GetDestination(int x, int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Tuple<int, int>(x - 1, y);
                case Direction.Up:
                    return new Tuple<int, int>(x, y + 1);
                case Direction.Right:
                    return new Tuple<int, int>(x + 1, y);
                case Direction.Down:
                    return new Tuple<int, int>(x, y - 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
        
        private static void Shuffle<T>(this IList<T> list)  
        {  
            var rng = new Random();
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}