
using System;

namespace Steamwar.Objects
{
    public abstract class DestroyableObject<T, S> : ObjectData<T, S> where T : ObjectType where S : DestroyableObjectSerializable, new()
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

        public uint Health { get; set; }
    }

    [Serializable]
    public class DestroyableObjectSerializable : ObjectDataSerializable
    {
        public uint health;
    }
}
