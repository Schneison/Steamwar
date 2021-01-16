using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

namespace Steamwar.Grid
{
    public interface ITileListener
    {
        /// <summary>
        /// Called once at the initialisation of the cell
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="pos"></param>
        /// <param name="info"></param>
        public void OnTileCreated(TileBase tile, BoardLayerType layer, ICellInfo info, Board board);

        /// <summary>
        /// Called after the tile of a cell changes.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="pos"></param>
        /// <param name="info"></param>
        /// <param name="board"></param>
        public void OnTileChanged((TileBase, TileBase) tiles, BoardLayerType layer, ICellInfo info, Board board);

        /// <summary>
        /// Called once at the deletion of the tile from the cell
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="name"></param>
        /// <param name="info"></param>
        /// <param name="board"></param>
        public void OnTileRemoved(TileBase tile, BoardLayerType layer, ICellInfo info, Board board);
    }
}
