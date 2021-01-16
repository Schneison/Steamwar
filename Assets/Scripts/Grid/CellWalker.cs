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
        public readonly int startPos;
        public readonly BitArray visited = new BitArray(Board.ARRAY_SIZE, false);
        public readonly int maxAmount;

        public CellWalker(Board board, Vector3Int startPos, int maxAmount)
        {
            this.board = board;
            this.startPos = Board.GetCellIndex(startPos);
            this.maxAmount = maxAmount;
        }

        public IEnumerator Walk()
        {
            int count = 0;
            Stack<int> posToVisit = new Stack<int>();
            void addIfValid(int currentPos)
            {
                ICellInfo info = board.GetCell(currentPos);
                if (!visited[currentPos] && info.Exists && BoardManager.AnyTile(currentPos, out (TileBase tile, BoardLayerType layer) tile))
                {
                    BoardManager.Instance.tileAdded.Invoke(tile.tile, tile.layer, info, board);

                    visited[currentPos] = true;
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
                int pos = posToVisit.Pop();

                addIfValid(Board.CellLeft(pos));
                addIfValid(Board.CellRight(pos));
                addIfValid(Board.CellUp(pos));
                addIfValid(Board.CellDown(pos));
                count++;
            }
        }
    }
}
