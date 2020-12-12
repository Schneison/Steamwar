using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

using Steamwar.Utils;

public class TileType : Tile
{
    public string id;
    public string displayName;

    [MenuItem("Create/Tile")]
    static void CreateType()
    {
        ScriptableObjectUtility.CreateAsset<TileType>();
    }
}