using Steamwar.Core;
using System;
using UnityEngine;

namespace Steamwar.Objects
{
    public abstract class ObjectContainer<D, T, S> : ObjectContainer, ISerializationCallbackReceiver where D : ObjectData<T, S>, new() where T : ObjectType where S : ObjectDataSerializable, new()
    {
        public S dataSerializable;

        public new D Data
        {
            get => _data as D;
        }

        public T Type
        {
            get => Data?.Type;
        }

        protected override void OnInit() {
            PropManager.CheckForProp(this);
        }

        protected abstract void Construction(T type);

        public override void OnConstruction(ObjectType type)
        {
            Construction(type as T);
            Board.Add(gameObject);
        }

        public void OnAfterDeserialize()
        {
            if(Data != null) {
                dataSerializable = Data.WriteData();
            }
        }

        public void OnBeforeSerialize()
        {
            if (Data != null)
            {
                Data.ReadData(dataSerializable);
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (_data == null)
            {
                _data = new D();
            }
            if (Data != null)
            {
                Data.ReadData(dataSerializable);
            }
        }

        public override void OnPropInit()
        {
            if (_data == null)
            {
                _data = new D();
            }
            if (Data != null)
            {
                Data.ReadData(dataSerializable);
            }
            ObjectElement element = GetComponent<ObjectElement>();
            if(element == null)
            {
                ConstructionManager.AddElement(this, Data.Type);
            }
            Board.Add(gameObject);
        }

        public override ObjectData GetOrLoadData()
        {
            if (_data == null)
            {
                _data = new D();
                ((D)_data).ReadData(dataSerializable);
            }
            return _data;
        }
    }

    public abstract class ObjectContainer : SteamBehaviour
    {
        public GameObject rendererChild;
        protected ObjectData _data;

        public abstract void OnConstruction(ObjectType type);

        /// <summary>
        /// If the object currently can move.
        /// </summary>
        public virtual bool CanMove { get => IsMovable; }


        /// <summary>
        /// If the object currently moves.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S3237:\"value\" parameters should be used", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>")]
        public virtual bool Moves { get => false; set{}
        }

        public bool HasAction(ActionType type)
        {
            return (GetAction() & type) == type;
        }

        /// <summary>
        /// If the object has the ability to move.
        /// </summary>
        public virtual bool IsMovable { get => false; }

        /// <summary>
        /// The data of this object. Will be converted to ObjectDataSerializable to save it to the disc.
        /// </summary>
        public ObjectData Data
        {
            get => _data;
        }

        public abstract ObjectData GetOrLoadData();

        /// <summary>
        /// The kind of the object this data represents.
        /// </summary>
        public abstract ObjectKind Kind
        {
            get;
        }

        /// <summary>
        /// Called for objects that are created by the sector and were not spawned by the player or an other faction. 
        /// 
        /// Gets called after all game systems were initialized so the object can safely create its data and do other actions that are based on these systems.
        /// </summary>
        public abstract void OnPropInit();

        public virtual void OnPrefabInit()
        {

        }

        /// <summary>
        /// All types that this object can handle.
        /// </summary>
        /// <returns>A multi flag type</returns>
        public abstract ActionType GetAction();

        /// <summary>
        /// The type that is selected if the player selects the unit.
        /// </summary>
        /// <returns>A single action type.</returns>
        public virtual ActionType GetDefaultType()
        {
            return ActionType.None;
        }

        public virtual void OnDisable()
        {
            Board.Remove(gameObject);
        }
    }
}
