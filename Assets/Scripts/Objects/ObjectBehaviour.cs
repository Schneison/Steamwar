using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Objects
{
    public class ObjectBehaviour<D, T, S> : MonoBehaviour, ISerializationCallbackReceiver where D : ObjectData<T, S>, new() where T : ObjectType where S : ObjectDataSerializable, new()
    {
        public D data;
        public S dataSerializable;

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
    }
}
