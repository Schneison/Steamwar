using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.IO;
using System.Reflection;

namespace Steamwar.Utils
{
    public static class ScriptableObjectUtility
    {
#if UNITY_EDITOR
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static void CreateAsset<T>() where T : ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = GetOpenFolder();
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            CreateAsset<T>(path + "/New " + typeof(T).ToString() + ".asset", (action)=>{ });
        }

        public static void CreateAsset<T>(string path, Action<T> action, bool updateAssets = true) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            action(asset);
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            if (updateAssets)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
        }

        public static string GetOpenFolder()
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            return obj.ToString();
        }
#endif
        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            return UnityEngine.Resources.FindObjectsOfTypeAll<T>();
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return a;
#else
            return UnityEngine.Resources.FindObjectsOfTypeAll<T>();
#endif
        }
    }
}
