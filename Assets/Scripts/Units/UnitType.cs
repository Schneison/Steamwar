using UnityEngine;
using UnityEditor;
using Steamwar.Utils;
using Steamwar.Resources;
using Steamwar.Objects;

namespace Steamwar.Units {

    public class UnitType : ObjectType
    {
        public RuntimeAnimatorController animation;
        public ResourceList cost;
        public float damage;
        public float health;
        public float speed;
        public int movment;

        [MenuItem("Create/Unit")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<UnitType>();
        }

    }
}