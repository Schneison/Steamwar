using Steamwar.Factions;
using Steamwar.Grid;
using Steamwar.Interaction;
using Steamwar.Objects;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Steamwar.Objects
{
    [Serializable]
    public class ConstructionLayer : BoardLayer
    {
        public TileBase areaTile;
        public TileBase constructAreaTile;

        public override void OnObjectConstructed(ObjectContainer obj)
        {
            obj.ActOnBuilding(
           (data, type, building) => type.Tag.HasFlag(ObjectTag.Construction),
           (data, type, building) =>
           {
               bool player = data.HasPlayerFaction();
               Vector3Int cellPos = BoardManager.UnitToCell(building.transform.position);
               ICellInfo info = BoardManager.GetInfo(cellPos);
               CellVendor vendor = new CellVendor();
               ConstructionProvider construction = type.construction;
               ICellInfo tileInfo;
               foreach (var pos in construction.GetTilePositions(cellPos))
               {
                   tileInfo = BoardManager.GetInfo(pos);
                   if (!tileInfo.Exists || tileInfo.IsEmpty)
                   {
                       continue;
                   }
                   tileInfo.Flags |= CellFlag.ConstructionBuilding;
                   CellCustomer customer = tileInfo.Pieces.Any() ? tileInfo.Pieces.Where((piece) => piece is CellCustomer).First() as CellCustomer : null;
                   if (customer == null)
                   {
                       tileInfo.AddPiece(customer = new CellCustomer());
                   }
                   customer.vendors.Add(tileInfo.Index);
                   vendor.customers.Add(tileInfo.Index);
                   if (!player)
                   {
                       continue;
                   }
                   BoardManager.Board.SetTile(pos, constructAreaTile, BoardLayerType.AreaOverlay);
               }
               info.AddPiece(vendor);
           });
        }

        public override void OnObjectDeconstructed(ObjectContainer obj)
        {

        }

        public override void OnTileCreated(TileBase tile, BoardLayerType layer, ICellInfo info, Board board)
        {
            Vector2Int pos = info.Pos ?? Vector2Int.left;
            if (info.Flags.HasFlag(CellFlag.ConstructionBuilding))
            {
                return;
            }
            board.SetTile(pos, areaTile, BoardLayerType.AreaOverlay);
        }

        public override void OnTileRemoved(TileBase tile, BoardLayerType layer, ICellInfo info, Board board)
        {
            Vector2Int pos = info.Pos ?? Vector2Int.left;
            board.RemoveTile(pos, BoardLayerType.AreaOverlay);
        }
    }
}