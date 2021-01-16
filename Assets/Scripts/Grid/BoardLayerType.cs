using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Grid
{
    [Flags]
    public enum BoardLayerType
    {
        None = 0,
        Ground = 1,
        Level1 =2,
        Level2 = 4,
        Level3 = 8,
        ChessOverlay = 16,
        AreaOverlay = 32,
        //64,
        //128,
    }

    public static class BorderLayerTypeExtensions
    {
        public static int GetIndex(this BoardLayerType source) => source switch
        {
            BoardLayerType.Ground => 0,
            BoardLayerType.Level1 => 1,
            BoardLayerType.Level2 => 2,
            BoardLayerType.Level3 => 3,
            BoardLayerType.ChessOverlay => 4,
            BoardLayerType.AreaOverlay => 5,
            _ => -1
        };
    }
}
