using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Objects
{
    [Serializable]
    public class ConstructionProvider
    {
        /// <summary>
        /// The type of object that can be constructed in the area.
        /// </summary>
        public ObjectKind constrcutionType;
        /// <summary>
        /// The size of the area this 
        /// </summary>
        public int areaSize;

        public IEnumerable<Vector3Int> GetTilePositions(Vector3Int centerPos)
        {
            for (int x = -areaSize; x <= areaSize; x++)
            {
                for (int y = -areaSize; y <= areaSize; y++)
                {
                    Vector3 pos = new Vector3(x, y, 0);
                    if(pos.magnitude > (areaSize + 0.5))
                    {
                        continue;
                    }
                    yield return centerPos + new Vector3Int(x, y, 0);
                }
            }
        }
    }
}
