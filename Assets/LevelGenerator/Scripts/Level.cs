using System;
using System.Collections.Generic;

namespace LevelGenerator.Scripts
{
    public class Level : ICloneable
    {
        private readonly int width;
        private readonly int height;
        internal int Width => width;
        internal int Height => height;

        internal const int StartPositionX = 1;
        internal const int StartPositionY = 1;

        internal int EndPositionX => Width - 2;
        internal int EndPositionY => Height - 2;

        private readonly Cell[,] cells;

        internal HashSet<Tuple<int, int>> Waypoints { get; private set; }
        internal bool[,] Paths { get; private set; }


        internal Level(int width, int height, bool random = false)
        {
            this.width = width;
            this.height = height;
            cells = new Cell[width, height];
            
            InitializeCells();
            GeneratePath(random);
            GeneratePathMatrix();
            GenerateWaypoints();
        }

        private Level(Level level)
        {
            width = level.Width;
            height = level.Height;
            cells = CloneCells(level.cells);
        }
        
        private void GeneratePath(bool random)
        {
            var generated = PathGenerator.Generate(this, random);
            for (int index = 0; index < cells.Length; index++)
            {
                var x = index % Width;
                var y = index / Width;
                cells[x, y] = generated.cells[x, y];
            }
        }

        private void GenerateWaypoints()
        {
            Waypoints = new HashSet<Tuple<int, int>>();
            foreach (var tuple in WaypointGenerator.GenerateWaypoints(this))
            {
                Waypoints.Add(tuple);
            }
        }
        
        private void GeneratePathMatrix()
        {
            Paths = new bool[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Paths[x, y] = GetCellContent(x, y) != CellContent.Node;
                }
            }
        }

        internal CellContent GetCellContent(int x, int y) => cells[x, y].Content;

        internal bool IsEmpty(int x, int y) => cells[x, y].IsEmpty;

        internal bool IsWaypoint(int x, int y) => Waypoints.Contains(new Tuple<int, int>(x, y));
        
        private Cell GetCell(int x, int y) => cells[x, y];
        
        private bool TryGetCell(int x, int y, out Cell cell)
        {
            if (IsInsideBoundaries(x, y))
            {
                cell = GetCell(x, y);
                return true;
            }
            cell = null;
            return false;
        }

        internal void SetPathAt(int x, int y) => cells[x, y].Content = CellContent.Path;
        
        private bool IsInsideBoundaries(int x, int y) =>
            Utils.IsInsideBoundaries(x, y, Width, Height);
        
        private void InitializeCells()
        {
            for (int index = 0; index < cells.Length; index++)
            {
                var x = index % Width;
                var y = index / Width;
                cells[x, y] = new Cell();
            }

            cells[StartPositionX, StartPositionY].Content = CellContent.Start;
            cells[EndPositionX, EndPositionY].Content = CellContent.End;
        }

        

        public object Clone() =>
            new Level(this);
        
        private Cell[,] CloneCells(Cell[,] source)
        {
            var clonedCells = new Cell[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    clonedCells[x, y] = (Cell) source[x, y].Clone();
                    //clonedCells[x, y] = new Cell(source[x, y].Content);
                }
            }
            return clonedCells;
        }

        private void CloneCells(Cell[,] source, out Cell[,] destination) => 
            destination = CloneCells(source);

        internal IEnumerable<string> ToAsp()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var name = cells[x, y].Content.ToString();
                    yield return AspGenerator.AspString(name, new List<string> {x.ToString(), y.ToString()});
                }
            }
        }
    }
}