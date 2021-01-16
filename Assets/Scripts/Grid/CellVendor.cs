using System.Collections.Generic;

namespace Steamwar.Grid
{
    public class CellVendor : CellPiece
    {
        public HashSet<int> customers = new HashSet<int>();
    }
}
