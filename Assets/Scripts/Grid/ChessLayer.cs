using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Steamwar.Grid
{
    public class ChessLayer : BoardLayer
    {
        public Tile overlayTile;
        public TileBase[] chessables;

        public override void OnTileCreated(TileBase tile, BoardLayerType layer, ICellInfo info, Board board)
        {
            Vector2Int pos = info.Pos??Vector2Int.left;
            int xDiff = Math.Abs(pos.x % 2);
            int yDiff = Math.Abs(pos.y % 2);
            if (xDiff == yDiff && chessables.Contains(tile))
            {
                board.SetTile(pos, overlayTile, BoardLayerType.ChessOverlay);
            }
        }

        public override void OnTileChanged((TileBase, TileBase) tiles, BoardLayerType layer, ICellInfo info, Board board)
        {

        }

        public override void OnTileRemoved(TileBase tile, BoardLayerType layer, ICellInfo info, Board board)
        {

        }
    }
}
