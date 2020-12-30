using Steamwar.Core;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;

namespace Steamwar.Objects
{
    /// <summary>
    /// Keeps track of every object that is currently on the board.
    /// </summary>
    public static class ObjectCache
    {
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

        public static IDictionary<ObjectTag, HashSet<int>> objectsByTag = new Dictionary<ObjectTag, HashSet<int>>();
        /// <summary>
        /// All objects on the boartd by there faction index.
        /// </summary>
        public static IDictionary<int, HashSet<int>> objectsByFaction = new Dictionary<int, HashSet<int>>();

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
            if (GameManager.ShuttDown())
            {
                return;
            }
            EventManager.Instance.objectConstrcuted.Invoke(obj);
            int id = obj.GetInstanceID();
            ObjectData data = obj.Data;
            objectsByKind.AddToSub(obj.Kind, id);
            if(data == null)
            {
                return;
            }
            ObjectType type = data.Type;
            objectByType.AddToSub(type, id);
            objectsByFaction.AddToSub(obj.Data.faction.index, id);
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
                    objectsByTag.AddToSub(objTag, id);
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
            if (GameManager.ShuttDown())
            {
                return;
            }
            EventManager.Instance.objectDeconstrcuted.Invoke(obj);
            int id = obj.GetInstanceID();
            ObjectData data = obj.Data;
            objectsByKind.RemoveFromSub(obj.Kind, id);
            if (data == null)
            {
                return;
            }
            ObjectType type = obj.Data.Type;
            objectByType.RemoveFromSub(type, id);
            objectsByFaction.RemoveFromSub(obj.Data.faction.index, id);
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
                    objectsByTag.RemoveFromSub(objTag, id);
                }
            }
        }

        public static IEnumerable<ObjectBehaviour> GetObjects(ObjectTag tag)
        {
            return from id in objectsByTag[tag]
                   select GetObject(id);
        }

        public static ObjectBehaviour GetObject(int index)
        {
            return objectById[index];
        }

        public static IEnumerable<ObjectBehaviour> GetObjectsFromFaction(int faction)
        {
            return from id in objectsByFaction.AddIfAbsent(faction)
                   select GetObject(id);
        }

        public static IEnumerable<(int, IEnumerable<ObjectBehaviour>)> GetObjectsByFactions()
        {
            return from pair in objectsByFaction
                   select (pair.Key, (from id in pair.Value select GetObject(id)));
        }

    }

    public struct CacheListener
    {

    }

    public struct CacheKey
    {
        private readonly ObjectTag tag;
        private readonly int index;

        public CacheKey(ObjectTag tag) : this()
        {
            this.tag = tag;
            this.index = -1;
        }

        public CacheKey(int index) : this()
        {
            this.tag = ObjectTag.None;
            this.index = index;
        }

        public bool IsTag
        {
            get
            {
                return tag != ObjectTag.None;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is CacheKey key &&
                   tag == key.tag &&
                   index == key.index;
        }

        public override int GetHashCode()
        {
            int hashCode = -1422057162;
            hashCode = hashCode * -1521134295 + tag.GetHashCode();
            hashCode = hashCode * -1521134295 + index.GetHashCode();
            return hashCode;
        }
    }
}
