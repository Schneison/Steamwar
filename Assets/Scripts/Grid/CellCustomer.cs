using System.Collections.Generic;

namespace Steamwar.Grid
{
    public class CellCustomer : CellPiece
    {
        public HashSet<int> vendors = new HashSet<int>();
    }
}
