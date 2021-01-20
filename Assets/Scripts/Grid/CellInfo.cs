using MyBox;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Steamwar.Grid
{
    public delegate byte TileIdCallback(string tileName);

    public delegate string TileNameCallback(byte id);

    public class CellInfo : ICellInfo
    {
        public const BoardLayerType SAVED_LAYERS_MASK = BoardLayerType.Ground | BoardLayerType.Level1 | BoardLayerType.Level2 | BoardLayerType.Level3;
        public static readonly ICellInfo Empty = new EmptyInfo();

        private readonly CellPos pos;
        public List<CellPiece> pieces;
        public BoardLayerType layers;
        public string[] tiles;

        public CellInfo(CellPos pos)
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

        public int Index { get => pos.PosIndex; }

        public CellPos? Pos => pos;

        public bool IsEmpty => Layers == BoardLayerType.None;

        public bool Exists => true;

        public void Serialize(BinaryWriter writer, TileIdCallback tileRegistry)
        {
            writer.Write((byte)(layers & SAVED_LAYERS_MASK));
            foreach(string tile in tiles)
            {
                writer.Write(tileRegistry(tile));
            }
        }

        public void Deserialize(BinaryReader reader, TileNameCallback tileRegistry)
        {
            int layers = reader.ReadByte();
            int i = 0;
            for (int mask = 0b1000; mask > 0; mask >>= 1)
            {
                if ((layers & mask) > 0)
                {
                    byte tileId = reader.ReadByte();
                    string tileName = tileRegistry(tileId);
                    if (!tileName.IsNullOrEmpty())
                    {
                        tiles[i] = tileRegistry(tileId);
                    }
                    else
                    {
                        layers &= ~mask;
                    }
                }
                i++;
            }
            this.layers = (BoardLayerType)layers;
        }

        public static ICellInfo Deserialize(BinaryReader reader, TileNameCallback tileRegistry, CellPos chunkPos)
        {
            int regionIndex = reader.ReadInt32();
            int layers = reader.ReadByte();
            CellPos pos = chunkPos.FromRegion(regionIndex);
            CellInfo instance = new CellInfo(pos);
            int i = 0;
            for (int mask = 0b1000; mask > 0; mask >>= 1)
            {
                if((layers & mask) > 0)
                {
                    byte tileId = reader.ReadByte();
                    string tileName = tileRegistry(tileId);
                    if (!tileName.IsNullOrEmpty())
                    {
                        instance.tiles[i] = tileRegistry(tileId);
                    }
                    else
                    {
                        layers &= ~mask;
                    }
                }
                i++;
            }
            instance.layers = (BoardLayerType)layers;
            return instance;
        }

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
