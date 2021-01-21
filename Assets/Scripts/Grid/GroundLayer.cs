using Steamwar.Grid;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Steamwar.Grid
{
    public class GroundLayer : BoardLayer
    {
        public override void OnTileCreated(TileBase tile, BoardLayerType layer, ICellInfo info, Board board)
        {
            board.SetTile((info.Pos??Vector2Int.zero).WithLayer(BoardLayerType.Ground), tile);
        }
    }
}