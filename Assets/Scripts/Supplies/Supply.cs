using UnityEngine;
using UnityEditor;
using Steamwar.Utils;

namespace Steamwar.Supplies
{
    public class Supply : ScriptableObject
    {
        public string id;
        public string displayName;
        public Sprite sprite;


#if UNITY_EDITOR
        [MenuItem("Create/Supply")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<Supply>();
        }
#endif
    }
}