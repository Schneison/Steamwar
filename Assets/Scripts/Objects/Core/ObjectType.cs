
using Steamwar.Buildings;
using UnityEngine;

namespace Steamwar.Objects
{
    public class ObjectType : ScriptableObject
    {
        public const uint FALLBACK_SPEED = 1;

        [Header("Identifiers")]
        public string id;
        public string displayName;
        [Header("Textures")]
        public Sprite spriteBlue;
        public Sprite spriteRed;
        public Sprite sprite;
        public Sprite[] baseSprites;
        public Sprite[] coloredSprites;
        [Range(0, 1)]
        public float speed;
        public GameObject elementPrefab;
        public ObjectTag _tag = ObjectTag.Undefined;

        public virtual uint Health
        {
            get
            {
                return 0;
            }
        }

        public float Speed
        {
            get
            {
                if (speed == 0)
                {
                    Debug.Log($"Found 'speed' value of the unit type with the name '{name}' with the value zero. This is bug, please report!");
                    speed = FALLBACK_SPEED;
                }
                return speed;
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
