using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Steamwar.Objects;
using Steamwar.Sectors;
using Steamwar.Supplies;

namespace Steamwar.Utils
{
    //https://forum.unity.com/threads/workflow-for-locating-scriptableobjects-at-runtime.502610/
    public class ResourcesIndex : ScriptableObject
    {
        private const string STORAGE_PATH = "ResourcesIndex";
        private static Dictionary<int, ResourceAsset> assetsDict;
#if UNITY_EDITOR
        // add types to be indexed here
        private static Dictionary<string, System.Type> typesToIndex = new Dictionary<string, System.Type>
    {
        // path in resources folder, type of UnityEngine.Object
         { "Objects/Buildings", typeof(ObjectType) },
         { "Objects/Units", typeof(ObjectType) },
         { "Objects/Sectors", typeof(Sector) },
         { "Objects/Supplies", typeof(Supply) },
         { "Objects/Factories", typeof(FactoryConfig) },
        //{ "Quests", typeof(QuestSO) },
        //{ "Characters", typeof(CharacterSO) },
        // etc... any types you want to index
    };
        [UnityEditor.InitializeOnLoadMethod]
        private static void BuildMap()
        {
            var index = Resources.Load<ResourcesIndex>(STORAGE_PATH);
            if (index == null)
            {
                index = CreateInstance<ResourcesIndex>();
                UnityEditor.AssetDatabase.CreateAsset(index, "Assets/Resources/" + STORAGE_PATH + ".asset");
            }
            index.assets = new List<ResourceAsset>();
            int id = 0;

            foreach (var kvp in typesToIndex)
            {
                var all = Resources.LoadAll(kvp.Key, kvp.Value);
                for (int i = 0; i < all.Length; i++)
                {
                    Object o = all[i];
                    index.assets.Add(new ResourceAsset()
                    {
                        id = id++,
                        assetPath = GetRelativeResourcePath(UnityEditor.AssetDatabase.GetAssetPath(o)),
                    });
                }
            }
            UnityEditor.EditorUtility.SetDirty(index);
        }
        public static string GetRelativeResourcePath(string path)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            if (path.Contains("/Resources/"))
            {
                string[] rSplit = path.Split(new string[] { "/Resources/" }, System.StringSplitOptions.RemoveEmptyEntries);
                string[] split = rSplit[1].Split('.');
                for (int j = 0; j < split.Length - 1; j++)
                {
                    stringBuilder.Append(split[j]);
                    if (j < split.Length - 2)
                        stringBuilder.Append('/');
                }
                return stringBuilder.ToString();
            }
            return path;
        }
        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false)
                BuildMap();
        }
#endif
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            var index = Resources.Load<ResourcesIndex>(STORAGE_PATH);
            assetsDict = new Dictionary<int, ResourceAsset>();
            for (int i = 0; i < index.assets.Count; i++)
            {
                ResourceAsset asset = index.assets[i];
                if (assetsDict.ContainsKey(asset.id))
                {
                    Debug.LogErrorFormat("Duplicate asset ids = {0}", asset.id);
                    continue;
                }
                assetsDict.Add(asset.id, asset);
            }
        }
        /// <summary>
        /// Returns objects of types T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetAsset<T>(int id) where T : Object
        {
            ResourceAsset asset;
            if (assetsDict.TryGetValue(id, out asset))
                return Resources.Load<T>(asset.assetPath);
            return null;
        }
        [System.Serializable]
        public struct ResourceAsset
        {
            public int id;
            public string assetPath;
        }
        public List<ResourceAsset> assets;
    }
}