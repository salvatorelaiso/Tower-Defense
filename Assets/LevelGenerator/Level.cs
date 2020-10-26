using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGenerator
{
    internal class Level : ICloneable
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
        private HashSet<Tuple<int, int>> waypoints;
        internal HashSet<Tuple<int, int>> Waypoints => waypoints;

        internal Level(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new Cell[width, height];
            
            InitializeCells();
        }

        private Level(Level level)
        {
            width = level.Width;
            height = level.Height;
            cells = CloneCells(level.cells);
        }
        
        internal void GeneratePath(bool random = false)
        {
            var generated = PathGenerator.Generate(this, random);
            for (int index = 0; index < cells.Length; index++)
            {
                var x = index % Width;
                var y = index / Width;
                cells[x, y] = generated.cells[x, y];
            }
        }

        internal void GenerateWaypoints()
        {
            waypoints = new HashSet<Tuple<int, int>>();
            foreach (var tuple in WaypointGenerator.GenerateWaypoints(this))
            {
                waypoints.Add(tuple);
            }
        }

        internal CellContent GetCellContent(int x, int y) => cells[x, y].Content;

        internal bool IsEmpty(int x, int y) => cells[x, y].IsEmpty;

        internal bool IsWaypoint(int x, int y) => waypoints.Contains(new Tuple<int, int>(x, y));
        
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

    }
}