using Steamwar.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Steamwar.Grid
{
    public class BoardChunk
    {
        public const int LENGHT = 8;
        public const int SIZE = LENGHT * LENGHT;
        public readonly CellPos chunkPos;
        private readonly CellInfo[] cells;
        private BitArray occupiedCells;

        public BoardChunk(CellPos chunkPos)
        {
            this.chunkPos = chunkPos;
            this.cells = new CellInfo[SIZE];
            this.occupiedCells = new BitArray(SIZE);
            /*for (int x = 0; x < LENGHT; x++)
            {
                for (int y = 0; y < LENGHT; y++)
                {
                    CellPos regionPos = chunkPos.Add(x, y);
                    this.cells[regionPos.RegionIndex] = new CellInfo(regionPos);
                }
            }*/
        }

        public BoardChunk(int chunkPos) : this(CellPos.FromChunk(chunkPos))
        {

        }

        public void Deserialize(BinaryReader reader, TileNameCallback tileRegistry)
        {
            byte[] cellOccupied = reader.ReadBytes(LENGHT);
            occupiedCells = new BitArray(cellOccupied);
            int lenght = reader.ReadInt32();
            byte[] cellBytes = reader.ReadBytes(lenght);
            using MemoryStream memoryStream = new MemoryStream(cellBytes);
            using BinaryReader cellReader = new BinaryReader(memoryStream);
            for (int i = 0; i < occupiedCells.Length; i++)
            {
                if (!occupiedCells.Get(i))
                {
                    continue;
                }
                if (cells[i] == null)
                {
                    cells[i] = new CellInfo(chunkPos.FromRegion(i));
                }
                cells[i].Deserialize(cellReader, tileRegistry);
            }
        }

        public void Serialize(BinaryWriter writer, TileIdCallback tileRegistry)
        {
            byte[] cellOccupied = new byte[LENGHT];
            occupiedCells.CopyTo(cellOccupied, 0);
            writer.Write(cellOccupied);
            int oldPos = (int)writer.BaseStream.Position;
            writer.Write(0);
            using MemoryStream memoryStream = new MemoryStream();
            using (BinaryWriter cellWriter = new BinaryWriter(memoryStream))
            {
                for (int i = 0; i < occupiedCells.Length; i++)
                {
                    if (!occupiedCells.Get(i))
                    {
                        continue;
                    }
                    cells[i].Serialize(cellWriter, tileRegistry);
                }
            }
            writer.Write(memoryStream.ToArray());
            int newPos = (int)writer.BaseStream.Position;
            writer.BaseStream.Position = oldPos;
            writer.Write(newPos - oldPos - 4);
            writer.BaseStream.Position = newPos;
        }

        public ICellInfo GetCell(CellPos pos, bool createEmpty = false)
        {
            int regionIndex = pos.RegionIndex;
            if(cells.Length > regionIndex)
            {
                ICellInfo info = cells[regionIndex];
                if(info == null)
                {
                    if (!createEmpty) {
                        return CellInfo.Empty;
                    }
                    cells[regionIndex] = new CellInfo(pos);
                    info = cells[regionIndex];
                }

                return info;
            }
            return CellInfo.Empty;
        }

        public bool HasCell(CellPos pos)
        {
            return GetCell(pos, false).Exists;
        }

        public bool RemoveTile(CellPos pos, bool createEmpty = false)
        {
            ICellInfo info = GetCell(pos, createEmpty);
            if (!info.Exists)
            {
                return false;
            }
            info.RemoveTile(pos.Layer);
            if (info.Layers == 0)
            {
                occupiedCells.Set(pos.RegionIndex, false);
            }
            return true;
        }

        public bool SetTile(CellPos pos, TileBase tile, bool createEmpty = true, bool updateRenderer = true)
        {
            ICellInfo info = GetCell(pos, createEmpty);
            if (!info.Exists)
            {
                return false;
            }
            if (updateRenderer) {
                BoardManager.SetTile(pos, tile, pos.Layer);
            }
            info.SetTile(Registry.GetName(tile), pos.Layer);
            if (info.Layers !=  0)
            {
                occupiedCells.Set(pos.RegionIndex, true);
            }
            return true;
        }

        public bool SetTile(CellPos pos, string tile, bool createEmpty = true)
        {
            ICellInfo info = GetCell(pos, createEmpty);
            if (!info.Exists)
            {
                return false;
            }
            info.SetTile(tile, pos.Layer);
            if (info.Layers != 0)
            {
                occupiedCells.Set(pos.RegionIndex, true);
            }
            return true;
        }

    }
}
