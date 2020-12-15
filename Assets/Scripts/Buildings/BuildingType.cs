using UnityEngine;
using UnityEditor;
using Steamwar.Utils;
using Steamwar.Objects;

namespace Steamwar.Buildings
{
    public class BuildingType : ObjectType
    {
        public const uint FALLBACK_HEALTH = 250;

        public float health;

        public float Health
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

        [MenuItem("Create/Building")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<BuildingType>();
        }
    }
}