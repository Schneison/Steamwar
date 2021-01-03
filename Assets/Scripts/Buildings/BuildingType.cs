using UnityEngine;
using UnityEditor;
using Steamwar.Utils;
using Steamwar.Objects;
using Steamwar.Resources;
using MyBox;

namespace Steamwar.Buildings
{
    public class BuildingType : ObjectType
    {
        public const uint FALLBACK_HEALTH = 10;

        [Range(0, 60)]
        public uint health;

        [Separator("Segments")]
        public bool hasStorage;

        //[HideInInspector]
        [ConditionalField(nameof(hasStorage))]
        public ResourceProps storageCapacity;

        public override uint Health
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

        protected override ObjectTag CreateTag()
        {
            ObjectTag tag = ObjectTag.None;
            if (hasStorage)
            {
                tag |= ObjectTag.Storage;
            }
            return tag;
        }

        [MenuItem("Create/Building")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<BuildingType>();
        }
    }
}