using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Utils
{
    public static class VectorExtensions
    {
        public static Vector3Int Floor(this Vector3  vector) { return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), Mathf.FloorToInt(vector.z)); }

        public static float ManhattanDistance(this Vector2 a, Vector2 b)
        {
     
       checked
            {
                return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
            }
        }

        public static Vector2Int Floor(this Vector2 vector)
        {
            return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
        }
    }
}
