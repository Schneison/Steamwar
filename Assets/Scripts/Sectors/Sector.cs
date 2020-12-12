using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Steamwar.Utils;

namespace Steamwar.Sectors
{
    [Serializable]
    public class Sector : ScriptableObject
    {
        public static readonly Vector2 DEFAULT_POSITION = new Vector2(0, 0);

        public string id;
        public string diplayName;
        public Vector2 center;
        public Vector2 size;
        public int roundsMax;
        public Sprite background;
        public string boardPath;

        public virtual void StartSector(Session game)
        {
            
        }

        public virtual void EndSector(Session game)
        {

        }

        internal SectorData ToData()
        {
            return new SectorData(this);
        }

        [MenuItem("Create/Sector")]
        static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<Sector>();
        }
    }
}
