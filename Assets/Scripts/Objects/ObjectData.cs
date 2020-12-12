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
    public abstract class ObjectData<T, S> where T : ObjectType where S : ObjectDataSerializable, new()
    {
        private Vector3 position;
        public Faction faction;
        public T type;
        public int hash;

        public Vector3 Position { get => position; set
            {
                position = value;
                hash = GetHash(value);
            }
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


        public override bool Equals(object obj)
        {
            return obj is ObjectData<T, S> data && position.Equals(data.position);
        }

        public override int GetHashCode()
        {
            return hash;
        }

        public abstract ObjectKind GetKind();

        public static int GetHash(Vector2 position)
        {
            Vector2Int pos2 = (position - new Vector2(0.5F, 0.5F)).Floor();
            return pos2.y & short.MaxValue | (pos2.x & short.MaxValue) << 16 | (pos2.x < 0 ? int.MinValue : 0);
        }
    }

}
