using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamwar.Units;
using Steamwar.Factions;
using Steamwar.Objects;

namespace Steamwar.Buildings
{
    public class BuildingData : ObjectData<BuildingType, BuildingDataSerializable>
    {
        public override ObjectKind GetKind()
        {
            return ObjectKind.BUILDING;
        }

        public virtual BuildingData Copy()
        {
            BuildingData data = new BuildingData();
            data.ReadData(WriteData());
            return data;
        }
    }

    [Serializable]
    public class BuildingDataSerializable : ObjectDataSerializable
    {

    }
}
