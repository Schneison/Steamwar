
using Steamwar.Buildings;
using Steamwar.Resources;
using UnityEngine;
using UnityEditor;
using MyBox;
using Steamwar.Utils;

namespace Steamwar.Objects
{
    public class ObjectType : ScriptableObject
    {
        public const uint FALLBACK_SPEED = 1;
        public const uint FALLBACK_HEALTH = 25;
        public const uint FALLBACK_MOVMENT = 3;

        [Header("Identifiers")]
        public string id;
        public string displayName;

        [Header("Textures")]
        public Sprite spriteBlue;
        public Sprite spriteRed;
        public Sprite sprite;
        public Sprite[] baseSprites;
        public Sprite[] coloredSprites;
        public RuntimeAnimatorController[] animations;
        public RuntimeAnimatorController[] coloredAnimations;

        public ObjectKind kind;

        [Separator("Segments")]
        public bool hasStorage;
        public bool isDestroyable;
        public bool isMovable;
        public bool canAttack;
        public bool canConstruct;

        [ConditionalField(nameof(isMovable))]
        [Range(0, 1)]
        public float speed;
        [ConditionalField(nameof(isMovable))]
        [Range(0, 20)]
        public uint movment;

        [ConditionalField(nameof(isDestroyable))]
        [Range(0, 60)]
        public uint health;

        [ConditionalField(nameof(canAttack))]
        [Range(0, 20)]
        public uint damage;

        //[HideInInspector]
        [ConditionalField(nameof(hasStorage))]
        public ResourceProps storageCapacity;

        [ConditionalField(nameof(canConstruct))]
        public ConstructionProvider construction;

        public GameObject elementPrefab;
        public ObjectTag _tag = ObjectTag.Undefined;

        public virtual uint Health
        {
            get
            {
                if (health == 0)
                {
                    Debug.Log($"Found 'health' value of the building type with the name '{name}' with the value zero. This is bug, please report!");
                    health = FALLBACK_HEALTH;
                }
                return health;
            }
        }

        public uint Movment
        {
            get
            {
                if (movment == 0)
                {
                    Debug.Log($"Found 'movment' value of the unit type with the name '{name}' with the value zero. This is bug, please report!");
                    health = FALLBACK_MOVMENT;
                }
                return movment;
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

        public ActionType GetDefaultAction()
        {
            if (isMovable)
            {
                return ActionType.Move;
            }
            return ActionType.None;
        }

        public ActionType GetAction()
        {
            ActionType type = ActionType.None;
            if (isMovable)
            {
                type |= ActionType.Move;
                type |= ActionType.Skip;
            }
            if(isDestroyable)
            {
                type |= ActionType.Repair;
                type |= ActionType.Destroy;
            }
            if(canAttack)
            {
                type |= ActionType.Attack;
                type |= ActionType.Skip;
            }

            return type;
        }


        [MenuItem("Create/Object Type")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<ObjectType>();
        }
    }
}
