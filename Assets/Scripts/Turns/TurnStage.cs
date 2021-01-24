using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Turns
{
    [Serializable]
    public enum TurnStage
    {
        PRE,
        START,
        FACTION_START,
        FACTION_END,
        END,
        BETWEEN
    }
}
