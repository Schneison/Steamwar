
using System;

namespace Steamwar.Objects
{
    public abstract class DestroyableData<T, S> : ObjectData<T, S> where T : ObjectType where S : DestroyableDataSerializable, new()
    {
        public override void WriteData(S serializable)
        {
            base.WriteData(serializable);
            serializable.health = Health;
        }

        public override void ReadData(S serializable)
        {
            base.ReadData(serializable);
            Health = serializable.health;
        }

        public bool IsAlive
        {
            get
            {
                return Health <= 0;
            }
        }

        public override uint Health { get; set; }
    }

    [Serializable]
    public class DestroyableDataSerializable : ObjectDataSerializable
    {
        public uint health;
    }
}
