using System;

namespace LevelGenerator
{
    internal enum CellContent { Node, Path, Start, End }
    
    internal class Cell : ICloneable
    {
        public CellContent Content { get; set; }

        internal Cell() =>
            Content = CellContent.Node;
        
        internal Cell(CellContent content) =>
            Content = content;
        
        internal bool IsEmpty =>
            Content == CellContent.Node;

        public object Clone() =>
            new Cell(Content);

    }
}