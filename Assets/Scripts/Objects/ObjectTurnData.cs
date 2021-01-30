using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    /// <summary>
    /// CContains data that is reset every turn. Like movment or unit attack.
    /// </summary>
    [Serializable]
    public class ObjectTurnData
    {
        /// <summary>
        /// The moves left for this object this turn.
        /// </summary>
        public uint moves;
        /// <summary>
        /// If this object already has attacked this turn.
        /// </summary>
        public bool attacked;
        /// <summary>
        /// If this object was has made an action this turn.
        /// </summary>
        public bool touched;
        /// <summary>
        /// If the turn of this object was skipped. If this is true 'touched' should be true too.
        /// </summary>
        public bool skiped;
    }
}
