using System.Collections.Generic;
using UnityEngine;

namespace Steamwar.Grid
{
    /// <summary>
    /// Contains all information of a cell on the board.
    /// </summary>
    public interface ICellInfo
    {
        /// <summary>
        /// Flags that descripe the content of this cell.
        /// </summary>
        public CellFlag Flags { get; set; }
        /// <summary>
        /// The objecct index of the object that is played on this cell if there is one.
        /// </summary>
        public int? ObjectIndex { get; }
        /// <summary>
        /// The piece that controlls this cell.
        /// </summary>
        public IEnumerable<CellPiece> Pieces { get; }
        /// <summary>
        /// The height of the heighest world layer of this cell.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The world laysers that contain a tile for this cell.
        /// </summary>
        public BoardLayerType Layers { get; }
        /// <summary>
        /// The tiles of this cell. The index equals the index of there layer in the Layers array.
        /// </summary>
        public IEnumerable<string> Tiles { get; }

        /// <summary>
        /// The position of the cell
        /// </summary>
        public int Index { get; }

        public Vector2Int? Pos { get; }

        /// <summary>
        /// If the cell contains no tile.
        /// </summary>
        public bool IsEmpty { get; }

        /// <summary>
        /// If the cell exists on the grid.
        /// </summary>
        public bool Exists { get; }

        public void AddPiece(CellPiece piece);

        public void RemovePiece(CellPiece piece);

        public void SetTile(string tile, BoardLayerType layer);

        public void RemoveTile(BoardLayerType layer, bool resize = true);

        public void OnObjectAdded(int objectIndex);

        public void OnObjectRemoved(int objectIndex);
    }
}
