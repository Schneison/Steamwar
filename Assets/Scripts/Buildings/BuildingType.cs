using UnityEngine;
using UnityEditor;
using Steamwar.Utils;
using Steamwar.Objects;

namespace Steamwar.Buildings
{
    public class BuildingType : ObjectType
    {
        [MenuItem("Create/Building")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<BuildingType>();
        }
    }
}