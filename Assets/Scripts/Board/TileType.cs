using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

using Steamwar.Utils;

namespace Steamwar.Board
{
    public class TileType : Tile
    {
        public string id;
        public string displayName;
        public bool chessable;

        [MenuItem("Create/Tile")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<TileType>();
        }
    }
}