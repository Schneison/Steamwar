using System;
using UnityEngine;
using Steamwar.Factions;
using Steamwar.Utils;
using System.Collections.Generic;

namespace Steamwar.Objects
{

    [Serializable]
    public class ObjectData : IFactionProvider
    {
        public Vector3 position;
        public int faction;
        public uint health;
        public float movment;
        [SerializeField]
        public ObjectType type;

        public Vector3 Position
        {
            get => position; 

            set {
                position = value;
            }
        }

        public virtual uint Health
        {
            get {
                return health;
            }

            set {
                health = value;
            }
        }

        public bool IsAlive => Health <= 0;

        /// <summary>
        /// The object type of this object.
        /// </summary>
        public ObjectType Type
        {
            get => type;
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// The kind of the object this data represents.
        /// </summary>
        public ObjectKind Kind => type.kind;

        public int FactionIndex
        {
            get => faction;
            set
            {
                faction = value;
            }
        }

        public static int GetHash(Vector2 position)
        {
            Vector2Int pos2 = (position - new Vector2(0.5F, 0.5F)).Floor();
            return pos2.y & short.MaxValue | (pos2.x & short.MaxValue) << 16 | (pos2.x < 0 ? int.MinValue : 0);
        }

        public override bool Equals(object obj)
        {
            return obj is ObjectData data &&
                   Position.Equals(data.Position) &&
                   EqualityComparer<ObjectType>.Default.Equals(Type, data.Type) &&
                   Kind == data.Kind &&
                   FactionIndex == data.FactionIndex;
        }

        public override int GetHashCode()
        {
            int hashCode = 431111939;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ObjectType>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + Kind.GetHashCode();
            hashCode = hashCode * -1521134295 + FactionIndex.GetHashCode();
            return hashCode;
        }
    }

}
