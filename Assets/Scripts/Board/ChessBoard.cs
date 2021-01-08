using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Steamwar.Board
{
    public class ChessBoard : MonoBehaviour
    {
        public const int OFFSET = 64;
        public Tilemap overlayGrid;
        public Tilemap groundGrid;
        public Tile overlayTile;
        public TileBase[] chessables;
        public Vector3Int startPos = Vector3Int.zero;
        public BitArray visited = new BitArray(0xFFFF, false);

        void Start()
        {
            StartCoroutine("OnTileUpdate");
        }

        private int GetIndex(Vector3Int pos)
        {
            return ((pos.x + OFFSET) << 8) | (pos.y + OFFSET);
        }

        public void OnTileUpdate()
        {
            Stack<Vector3Int> posToVisit = new Stack<Vector3Int> ();
            void addIfValid(Vector3Int currentPos)
            {
                if (groundGrid.HasTile(currentPos) && !visited[GetIndex(currentPos)])
                {
                    visited[GetIndex(currentPos)] = true;
                    posToVisit.Push(currentPos);
                }
            }
            posToVisit.Push(startPos);
            while (posToVisit.Count != 0)
            {
                Vector3Int pos = posToVisit.Pop();
                TileBase tile = groundGrid.GetTile(pos);
                //TileType tile = groundGrid.GetTile<TileType>(pos);
                int xDiff = Math.Abs(pos.x % 2);
                int yDiff = Math.Abs(pos.y % 2);
                if (xDiff == yDiff /*&& tile != null && tile.chessable*/ && chessables.Contains(tile))
                {
                    overlayGrid.SetTile(new Vector3Int(pos.x, pos.y, 0), overlayTile);
                }
                addIfValid(pos + Vector3Int.up);
                addIfValid(pos + Vector3Int.down);
                addIfValid(pos + Vector3Int.left);
                addIfValid(pos + Vector3Int.right);
            }
        }
    }
}
