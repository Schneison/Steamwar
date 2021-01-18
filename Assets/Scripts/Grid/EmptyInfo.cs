using System.Collections.Generic;
using UnityEngine;

namespace Steamwar.Grid
{
    public class EmptyInfo : ICellInfo
    {
        internal EmptyInfo()
        {

        }

        public CellFlag Flags {
            get
            {
                return CellFlag.None;
            }

            set
            {

            }
        }

        public int? ObjectIndex => null;

        public IEnumerable<CellPiece> Pieces => new CellPiece[0];

        public int Height => -1;

        public BoardLayerType Layers => BoardLayerType.None;

        public IEnumerable<string> Tiles => new string[0];

        public int Index => -1;

        public CellPos? Pos => null;

        public bool IsEmpty => true;

        public bool Exists => false;

        public void AddPiece(CellPiece piece)
        {
        }

        public void SetTile(string tile, BoardLayerType layer)
        {
        }

        public void OnObjectAdded(int objectIndex)
        {
        }

        public void OnObjectRemoved(int objectIndex)
        {
        }

        public void RemovePiece(CellPiece piece)
        {
        }

        public void RemoveTile(BoardLayerType layer, bool resize = true)
        {
        }

        public override string ToString()
        {
            return $"EmptyCell()";
        }
    }
}
