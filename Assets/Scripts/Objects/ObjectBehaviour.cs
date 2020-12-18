using Steamwar.Core;
using UnityEngine;

namespace Steamwar.Objects
{
    public abstract class ObjectBehaviour<D, T, S> : ObjectBehaviour, ISerializationCallbackReceiver where D : ObjectData<T, S>, new() where T : ObjectType where S : ObjectDataSerializable, new()
    {
        public D data;
        public S dataSerializable;

        public override ObjectData Data
        {
            get => data;
        }

        public virtual void Start() {
            PropManager.CheckForProp(this);
        }

        protected abstract void Construction(T type);

        public override void OnConstruction(ObjectType type)
        {
            Construction(type as T);
        }

        public void OnAfterDeserialize()
        {
            if(data != null) {
                dataSerializable = data.WriteData();
            }
        }

        public void OnBeforeSerialize()
        {
            if (data != null)
            {
                data.ReadData(dataSerializable);
            }
        }


        public override void OnPropInit()
        {
            if (data == null)
            {
                data = new D();
            }
            if (data != null)
            {
                data.ReadData(dataSerializable);
            }
        }
    }

    public abstract class ObjectBehaviour : MonoBehaviour
    {
        public abstract void OnConstruction(ObjectType type);

        /// <summary>
        /// If the object is currently selected by the player.
        /// </summary>
        public bool Selected { get; private set; }

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

        /// <summary>
        /// If the object has the ability to move.
        /// </summary>
        public virtual bool IsMovable { get => false; }

        /// <summary>
        /// Selects the object for the player
        /// </summary>
        /// <returns>True if the object was selected.</returns>
        public bool Select()
        {
            if(!IsSelectable() || Selected)
            {
                return false;
            }
            OnSelection();
            Selected = true;
            return true;
        }

        /// <summary>
        /// Called at the moment the object gets selected
        /// </summary>
        public virtual void OnSelection()
        {
            // Empty by default
        }

        /// <summary>
        /// If the object can be selected by the player.
        /// </summary>
        /// <returns>True if the object can be selected.</returns>
        protected virtual bool IsSelectable()
        {
            return true;
        }

        /// <summary>
        /// Deselects the object for the player
        /// </summary>
        /// <returns>True if the object was deselected.</returns>
        public bool Deselect()
        {
            if (!IsDeselectable() || !Selected)
            {
                return false;
            }
            Selected = false;
            OnDeselection();
            return true;
        }

        /// <summary>
        /// If the object can be selected by the player.
        /// </summary>
        /// <returns>True if the object can be selected.</returns>
        protected virtual bool IsDeselectable()
        {
            return true;
        }

        /// <summary>
        /// Called at the moment the object gets deselected
        /// </summary>
        public virtual void OnDeselection()
        {
            // Empty by default
        }

        /// <summary>
        /// The data of this object. Will be converted to ObjectDataSerializable to save it to the disc.
        /// </summary>
        public abstract ObjectData Data
        {
            get;
        }

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
    }
}
