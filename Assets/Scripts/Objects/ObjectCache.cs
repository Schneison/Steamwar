using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Objects
{
    /// <summary>
    /// Keeps track of every object that is currently on the board.
    /// </summary>
    public static class ObjectCache
    {
        public delegate void ObjectAdded(ObjectBehaviour obj);

        public delegate void ObjectRemoved(ObjectBehaviour obj);

        /// <summary>
        /// All objects on the board by there kind
        /// </summary>
        public static IDictionary<ObjectKind, HashSet<int>> objectsByKind = new Dictionary<ObjectKind, HashSet<int>>();
        /// <summary>
        /// All objects on the board by there own unique id
        /// </summary>
        public static IDictionary<int, ObjectBehaviour> objectById = new Dictionary<int, ObjectBehaviour>();
        /// <summary>
        /// All objects on the board by there type
        /// </summary>
        public static IDictionary<ObjectType, HashSet<int>> objectByType = new Dictionary<ObjectType, HashSet<int>>();
        /// <summary>
        /// All objects on the board by there name. Can later by used for "scenarios" ?
        /// </summary>
        //public static IDictionary<string, ObjectBehaviour> objectByName = new Dictionary<string, ObjectBehaviour>();

        public static IDictionary<ObjectTag, HashSet<int>> objerctByTag = new Dictionary<ObjectTag, HashSet<int>>();

        private static event ObjectAdded OnCreation;

        private static event ObjectRemoved OnDeletion;

        public static void Listen(ObjectAdded callback)
        {
            OnCreation += callback;
        }

        public static void Unlisten(ObjectAdded callback)
        {
            OnCreation -= callback;
        }

        public static void Listen(ObjectRemoved callback)
        {
            OnDeletion += callback;
        }

        public static void Unlisten(ObjectRemoved callback)
        {
            OnDeletion -= callback;
        }

        public static void Add(GameObject gameObject)
        {
            ObjectBehaviour obj = gameObject.GetComponent<ObjectBehaviour>();
            if(obj != null)
            {
                Add(obj);
            }
        }

        public static void Add(ObjectBehaviour obj)
        {
            int id = obj.GetInstanceID();
            ObjectData data = obj.Data;
            objectsByKind.AddToSub(obj.Kind, id);
            if(data == null)
            {
                return;
            }
            ObjectType type = obj.Data.Type;
            objectByType.AddToSub(type, id);
            ObjectTag tag = type.Tag;
            if (tag == ObjectTag.None)
            {
                return;
            }
            objectById[id] = obj;
            foreach(ObjectTag objTag in Enum.GetValues(typeof(ObjectTag)))
            {
                if((tag & objTag) == objTag)
                {
                    objerctByTag.AddToSub(objTag, id);
                }
            }
        }

        public static void Remove(GameObject gameObject)
        {
            ObjectBehaviour obj = gameObject.GetComponent<ObjectBehaviour>();
            if (obj != null)
            {
                Remove(obj);
            }
        }

        public static void Remove(ObjectBehaviour obj)
        {
            int id = obj.GetInstanceID();
            ObjectData data = obj.Data;
            objectsByKind.RemoveFromSub(obj.Kind, id);
            if (data == null)
            {
                return;
            }
            ObjectType type = obj.Data.Type;
            objectByType.RemoveFromSub(type, id);
            objectById.Remove(id);
            ObjectTag tag = type.Tag;
            if (tag == ObjectTag.None)
            {
                return;
            }
            foreach (ObjectTag objTag in Enum.GetValues(typeof(ObjectTag)))
            {
                if ((tag & objTag) == objTag)
                {
                    objerctByTag.RemoveFromSub(objTag, id);
                }
            }
        }

    }
}
