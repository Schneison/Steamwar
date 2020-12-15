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
        public float health;

        public override ObjectKind Kind
        {
            get=>ObjectKind.BUILDING;
        }

        public override void WriteData(BuildingDataSerializable serializable)
        {
            base.WriteData(serializable);
            serializable.health = health;
        }

        public override void ReadData(BuildingDataSerializable serializable)
        {
            base.ReadData(serializable);
            health = serializable.health;
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
        public float health;
    }
}
