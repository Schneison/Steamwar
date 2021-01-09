using Steamwar.Core;
using System;
using UnityEngine;

namespace Steamwar.Objects
{

    public class ObjectContainer : SteamBehaviour
    {
        [SerializeField]
        private ObjectData data;

        public bool HasAction(ActionType type)
        {
            return (Type.GetAction() & type) == type;
        }

        /// <summary>
        /// If the object has the ability to move.
        /// </summary>
        public virtual bool IsMovable { get => false; }

        /// <summary>
        /// The data of this object. Will be converted to ObjectDataSerializable to save it to the disc.
        /// </summary>
        public ObjectData Data => data;


        /// <summary>
        /// The kind of the object this data represents.
        /// </summary>
        public ObjectKind Kind => Data.Kind;

        public ObjectType Type => Data.Type;

        /// <summary>
        /// Called for objects that are created by the sector and were not spawned by the player or an other faction. 
        /// 
        /// Gets called after all game systems were initialized so the object can safely create its data and do other actions that are based on these systems.
        /// </summary>
        public void OnPropInit()
        {
            if (data == null)
            {
                data = new ObjectData();
            }
            ObjectElement element = GetComponent<ObjectElement>();
            if (element == null)
            {
                ConstructionManager.AddElement(this, Data.Type);
            }
            Board.Add(gameObject);
        }

        public virtual void OnPrefabInit()
        {

        }

        protected override void OnInit()
        {
            PropManager.CheckForProp(this);
        }

        public void OnConstruction(ObjectType type)
        {
            data = new ObjectData
            {
                Type = type,
                Health = type.Health,
                faction = SessionManager.session.playerIndex
            };
            Board.Add(gameObject);
        }

        public virtual void OnDisable()
        {
            Board.Remove(gameObject);
        }
    }
}
