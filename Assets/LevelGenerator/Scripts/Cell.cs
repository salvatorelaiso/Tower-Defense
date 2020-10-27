using System;

namespace LevelGenerator.Scripts
{
    internal enum CellContent { Node, Path, Start, End }
    
    internal class Cell : ICloneable
    {
        public CellContent Content { get; set; }

        internal Cell() =>
            Content = CellContent.Node;
        
        private Cell(CellContent content) =>
            Content = content;
        
        internal bool IsEmpty =>
            Content == CellContent.Node;

        public object Clone() =>
            new Cell(Content);
        
    }
}