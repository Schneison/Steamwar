
using Steamwar.Buildings;
using UnityEngine;

namespace Steamwar.Objects
{
    public class ObjectType : ScriptableObject
    {
        [Header("Identifiers")]
        public string id;
        public string displayName;
        [Header("Textures")]
        public Sprite spriteBlue;
        public Sprite spriteRed;
        public Sprite sprite;
        public GameObject elementPrefab;
        public ObjectTag _tag = ObjectTag.Undefined;

        public virtual uint Health
        {
            get
            {
                return 0;
            }
        }

        public ObjectTag Tag
        {
            get
            {
                if(_tag == ObjectTag.Undefined)
                {
                    _tag = CreateTag();
                }
                return _tag;
            }
        }

        protected virtual ObjectTag CreateTag()
        {
            return ObjectTag.None;
        }
    }
}
