using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Steamwar.Factions;
using Steamwar.Utils;

namespace Steamwar.Objects
{
    public abstract class ObjectData<T, S> : ObjectData where T : ObjectType where S : ObjectDataSerializable, new()
    {
        public T type;

        public override ObjectType Type
        {
            get => type;
            set => type = value as T;
        }

        public virtual void WriteData(S serializable)
        {
            serializable.position = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            serializable.type = type.id;
            serializable.Faction = faction;
        }

        public S WriteData()
        {
            S serializable = new S();
            WriteData(serializable);
            return serializable;
        }

        public virtual void ReadData(S serializable)
        {
            Position = new Vector3(serializable.position.x, serializable.position.y, 0);
            type = SessionManager.registry.GetType<T>(serializable.type);
            faction = serializable.Faction;
        }

    }

    public abstract class ObjectData {
        internal Vector3 position;
        public Faction faction;
        public int hash;

        public Vector3 Position
        {
            get => position; 

            set {
                position = value;
                hash = GetHash(value);
            }
        }


        public override bool Equals(object obj)
        {
            return obj is ObjectData data && position.Equals(data.position);
        }

        public override int GetHashCode()
        {
            return hash;
        }

        /// <summary>
        /// The object type of this object.
        /// </summary>
        public abstract ObjectType Type
        {
            get;
            set;
        }

        /// <summary>
        /// The kind of the object this data represents.
        /// </summary>
        public abstract ObjectKind Kind
        {
            get;
        }

        public static int GetHash(Vector2 position)
        {
            Vector2Int pos2 = (position - new Vector2(0.5F, 0.5F)).Floor();
            return pos2.y & short.MaxValue | (pos2.x & short.MaxValue) << 16 | (pos2.x < 0 ? int.MinValue : 0);
        }
    }

}
