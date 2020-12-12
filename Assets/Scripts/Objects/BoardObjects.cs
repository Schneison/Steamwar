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
        public List<UnitData> units;
        public List<BuildingData> buildings;

        internal BoardObjects Copy()
        {
            return new BoardObjects
            {
                units = new List<UnitData>(units),
                buildings = new List<BuildingData>(buildings)
            };
        }
    }
}
