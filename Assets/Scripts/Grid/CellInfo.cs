using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Steamwar.Grid
{
    public class CellInfo : ICellInfo
    {
        public static readonly ICellInfo Empty = new EmptyInfo();

        private readonly int pos;
        public List<CellPiece> pieces;
        public BoardLayerType layers;
        public string[] tiles;

        public CellInfo(int pos)
        {
            this.pos = pos;
            this.layers = BoardLayerType.None;
            this.tiles = new string[0];
            this.Height = 0;
            this.ObjectIndex = null;
            this.Flags = CellFlag.None;
            this.pieces = new List<CellPiece>();
        }

        public CellFlag Flags { get; set; }
        public int? ObjectIndex { get; private set; }
        public IEnumerable<CellPiece> Pieces { get => pieces; }
        public int Height { get; private set; }

        public BoardLayerType Layers { get => layers; }

        public IEnumerable<string> Tiles { get => tiles; }

        public int Index { get => pos; }

        public Vector2Int? Pos => Board.GetPosFromIndex(Index);

        public bool IsEmpty => Layers == BoardLayerType.None;

        public bool Exists => true;

        public void AddPiece(CellPiece piece)
        {
            pieces.Add(piece);
        }
        public void RemovePiece(CellPiece piece)
        {
            pieces.Remove(piece);
        }

        public override bool Equals(object obj)
        {
            return obj is CellInfo info &&
                   pos == info.pos;
        }

        public override int GetHashCode()
        {
            return 991532785 + pos.GetHashCode();
        }

        public void OnObjectAdded(int objectIndex)
        {
            ObjectIndex = objectIndex;
        }

        public void OnObjectRemoved(int objectIndex)
        {
            ObjectIndex = -1;
        }
        public void SetTile(string tile, BoardLayerType layer)
        {
            int index = layer.GetIndex();
            if (tiles.Length < (index + 1))
            {
                Array.Resize(ref tiles, (index + 1) > 1 ? (index + 1) > 4 ? 8 : 4 : 1);
            }
            tiles[index] = tile;
            layers |= layer;
        }

        public void RemoveTile(BoardLayerType layer, bool resize = true)
        {
            tiles[layer.GetIndex()] = null;
            if (resize)
            {
                int count = tiles.Count((value) => !value.IsNullOrEmpty());
                if (tiles.Length == 8 && count <= 4)
                {
                    Array.Resize(ref tiles, 4);
                }
                else if (tiles.Length == 4 && count == 1)
                {
                    Array.Resize(ref tiles, 1);
                }
            }
            layers &= ~layer;
        }

        public override string ToString()
        {
            return $"Cell(pos={pos},tiles{{{string.Join(", ", tiles.ToArray())}}},layers={layers},flags={Flags})";
        }
    }
}
