using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    [Flags]
    public enum ObjectTag
    {
        Undefined = -1,
        None = 0,
        Storage = 1,
        Movable = 2,
        Construction = 4
    }
}
