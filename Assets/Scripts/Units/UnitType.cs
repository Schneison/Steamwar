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
        public const uint FALLBACK_MOVMENT = 3;

        public RuntimeAnimatorController animation;
        public ResourceList cost;
        public uint damage;
        public uint health;
        public float speed;
        public uint movment;

        public float Speed {
            get {
                if (speed == 0)
                {
                    Debug.Log($"Found 'speed' value of the unit type with the name '{name}' with the value zero. This is bug, please report!");
                    speed = FALLBACK_SPEED;
                }
                return speed;
            }
        }

        public uint Health
        {
            get
            {
                if (health == 0)
                {
                    Debug.Log($"Found 'health' value of the unit type with the name '{name}' with the value zero. This is bug, please report!");
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

        [MenuItem("Create/Unit")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<UnitType>();
        }



    }
}