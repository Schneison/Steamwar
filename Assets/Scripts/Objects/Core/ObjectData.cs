using System;
using UnityEngine;
using Steamwar.Factions;
using Steamwar.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Steamwar.Objects
{

    [Serializable]
    public class ObjectData : IFactionProvider
    {
        [SerializeField]
        private Vector3 position;
        [SerializeField]
        private int faction;
        [SerializeField]
        private uint health;
        [SerializeField]
        public ObjectType type;
        [SerializeField]
        public ObjectTurnData turnData;

        public ObjectData Copy() {
            return new ObjectData { position = position, faction = faction, health = health, type = type };
        }

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

        public bool CanMove => turnData?.moves > 0;

        public bool CanAttack => !turnData?.attacked ?? false;

        public void OnMove(uint moveAmount, Vector3 position)
        {
            Assert.IsNotNull(turnData);
            turnData.moves -= moveAmount;
            Position = position;
        }

        public void OnSkip()
        {
            Assert.IsNotNull(turnData);
            turnData.skiped = true;
            turnData.touched = true;
        }

        public void OnAttack()
        {
            Assert.IsNotNull(turnData);
            turnData.attacked = true;
            turnData.touched = true;
        }

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

        public void SetupTurn()
        {
            turnData = new ObjectTurnData
            {
                touched = false,
                skiped = false,
                attacked = false,
                moves = Type.movment
            };
        }

        public bool CanEndTurn()
        {
            return turnData == null || turnData.touched;
        }

        public void CleanupTurn()
        {
            turnData = null;
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
