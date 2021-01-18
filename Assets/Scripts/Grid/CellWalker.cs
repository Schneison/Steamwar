using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Steamwar.Grid
{
    public class CellWalker
    {
        public readonly Board board;
        public readonly CellPos startPos;
        public readonly BitArray visited = new BitArray(Board.ARRAY_SIZE, false);
        public readonly int maxAmount;

        public CellWalker(Board board, Vector3Int startPos, int maxAmount)
        {
            this.board = board;
            this.startPos = new CellPos(startPos);
            this.maxAmount = maxAmount;
        }

        public IEnumerator Walk()
        {
            int count = 0;
            Stack<CellPos> posToVisit = new Stack<CellPos>();
            void addIfValid(CellPos currentPos)
            {
                if(!BoardManager.AnyTile(currentPos, out (TileBase tile, BoardLayerType layer) tile))
                {
                    return;
                }
                ICellInfo info = board.GetCell(currentPos, true);
                if (!visited[currentPos.PosIndex] && info.Exists)
                {
                    BoardManager.Instance.tileAdded.Invoke(tile.tile, tile.layer, info, board);

                    visited[currentPos.PosIndex] = true;
                    posToVisit.Push(currentPos);
                }
            }
            addIfValid(startPos);
            while (posToVisit.Count != 0)
            {
                if(count >= maxAmount)
                {
                    yield return null;
                    count = 0;
                }
                CellPos pos = posToVisit.Pop();

                addIfValid(pos.Left());
                addIfValid(pos.Right());
                addIfValid(pos.Up());
                addIfValid(pos.Down());
                count++;
            }
        }
    }
}
