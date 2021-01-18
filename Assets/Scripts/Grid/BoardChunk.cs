using Steamwar.Utils;
using System;
using System.Collections.Generic;
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
        private readonly CellInfo[] cells;

        public BoardChunk(int chunkPos)
        {
            CellPos pos = CellPos.FromChunk(chunkPos);
            this.cells = new CellInfo[SIZE];
            for (int x = 0; x < LENGHT; x++)
            {
                for (int y = 0; y < LENGHT; y++)
                {
                    CellPos regionPos = pos.Add(x, y);
                    this.cells[regionPos.RegionIndex] = new CellInfo(regionPos.PosIndex);
                }
            }
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
                    cells[regionIndex] = new CellInfo(pos.PosIndex);
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
            return true;
        }

        public bool SetTile(CellPos pos, TileBase tile, bool createEmpty = true)
        {
            ICellInfo info = GetCell(pos, createEmpty);
            if (!info.Exists)
            {
                return false;
            }
            BoardManager.SetTile(pos, tile, pos.Layer);
            info.SetTile(Registry.GetName(tile), pos.Layer);
            return true;
        }

    }
}
