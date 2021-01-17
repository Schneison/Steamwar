using UnityEngine;
using UnityEditor;

namespace Steamwar.Entities
{
    public class EntityType : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("Tools/MyTool/Do It in C#")]
        static void DoIt()
        {
            EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
#endif
    }
}