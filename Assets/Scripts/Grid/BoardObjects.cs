using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Steamwar.Buildings;
using Steamwar.Objects;
using Steamwar.Units;

namespace Steamwar.Grid
{
    [Serializable]
    public class BoardObjects
    {
        public List<ObjectData> objects;

        internal BoardObjects Copy()
        {
            return new BoardObjects
            {
                objects = new List<ObjectData>(objects),
            };
        }
    }
}
