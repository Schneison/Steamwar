using Steamwar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Objects
{
    public abstract class ObjectBehaviour<D, T, S> : ObjectBehaviour, ISerializationCallbackReceiver where D : ObjectData<T, S>, new() where T : ObjectType where S : ObjectDataSerializable, new()
    {
        public D data;
        public S dataSerializable;

        public virtual void Start() {
            PropManager.CheckForProp(this);
        }

        public virtual void Init(T type)
        {
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
        public abstract ObjectKind GetKind();

        /// <summary>
        /// Called for objects that are created by the sector and were not spawned by the player or an other faction. 
        /// 
        /// Gets called after all game systems were initialized so the object can safely create its data and do other actions that are based on these systems.
        /// </summary>
        public abstract void OnPropInit();
    }
}
