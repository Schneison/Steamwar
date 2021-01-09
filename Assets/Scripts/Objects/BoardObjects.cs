using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Steamwar.Buildings;
using Steamwar.Units;

namespace Steamwar.Objects
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
