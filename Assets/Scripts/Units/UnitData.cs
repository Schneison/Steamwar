using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamwar.Factions;
using Steamwar.Objects;

namespace Steamwar.Units
{
    public class UnitData : DestroyableObject<UnitType, UnitDataSerializable>
    {
        public float movment;

        /// <summary>
        /// If the unit can be moved only based on its state independently from its faction.
        /// </summary>
        public bool CanMove { get => movment > 0; }

        public override ObjectKind Kind { get => ObjectKind.UNIT; }

        public override void WriteData(UnitDataSerializable serializable)
        {
            base.WriteData(serializable);
            serializable.movment = movment;
        }

        public override void ReadData(UnitDataSerializable serializable)
        {
            base.ReadData(serializable);
            Health = serializable.health;
            movment = serializable.movment;
        }

        public virtual UnitData Copy()
        {
            UnitData data = new UnitData();
            data.ReadData(WriteData());
            return data;
        }
    }

    [Serializable]
    public class UnitDataSerializable : DestroyableObjectSerializable
    {
        public float movment;
    }
}
