using Steamwar.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Serialization
{
    public class Surrogates
    {
        public class Vector3Surrogate : ISerializationSurrogate
        {
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {
                Vector3 v3 = (Vector3)obj;
                info.AddValue("x", v3.x);
                info.AddValue("y", v3.y);
                info.AddValue("z", v3.z);
            }

            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {
                Vector3 v3 = (Vector3)obj;
                v3.x = (float)info.GetValue("x", typeof(float));
                v3.y = (float)info.GetValue("y", typeof(float));
                v3.z = (float)info.GetValue("z", typeof(float));
                obj = v3;
                return obj;
            }
        }

        public class ObjectTypeSurrogate : ISerializationSurrogate
        {
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {
                ObjectType type = (ObjectType)obj;
                info.AddValue("name", type.name);
                info.AddValue("kind", type.kind);
            }

            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {
                ObjectType type = (ObjectType)obj;
                string name = info.GetString("name");
                ObjectKind kind = (ObjectKind)info.GetValue("kind", typeof(ObjectKind));
                return SessionManager.registry.GetType(name, kind);
            }
        }
    }
}
