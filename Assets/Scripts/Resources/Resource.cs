using UnityEngine;
using UnityEditor;
using Steamwar.Utils;

namespace Steamwar.Resources {
    public class Resource : ScriptableObject
    {
        public string id;
        public string displayName;
        public Sprite sprite;


        [MenuItem("Create/Resource")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<Resource>();
        }
    }
}