using UnityEngine;
using UnityEditor;
using Steamwar.Utils;
using Steamwar.Resources;
using Steamwar.Objects;

namespace Steamwar.Units {

    public class UnitType : ObjectType
    {
        public const uint FALLBACK_SPEED = 1;
        public const uint FALLBACK_HEALTH = 25;

        public RuntimeAnimatorController animation;
        public ResourceList cost;
        public float damage;
        public float health;
        public float speed;
        public int movment;

        public float Speed {
            get {
                if (speed == 0)
                {
                    Debug.Log($"Found 'speed' value of the  unit type with the name '{name}' with the value zero. This is bug, please report!");
                    speed = FALLBACK_SPEED;
                }
                return speed;
            }
        }

        public float Health
        {
            get
            {
                if (health == 0)
                {
                    Debug.Log($"Found 'health' value of the  unit type with the name '{name}' with the value zero. This is bug, please report!");
                    health = FALLBACK_HEALTH;
                }
                return health;
            }
        }

        [MenuItem("Create/Unit")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<UnitType>();
        }



    }
}