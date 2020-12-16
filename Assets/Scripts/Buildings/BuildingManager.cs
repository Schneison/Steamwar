using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Buildings
{
    public class BuildingManager : Singleton<BuildingManager>
    {

        public static void CreateBuildingFromData(BuildingData data)
        {
            GameObject obj = new GameObject(data.type.id, new Type[] {typeof(SpriteRenderer), typeof(Rigidbody2D)});
            obj.transform.position = data.Position;
            obj.transform.parent = Instance.transform;
            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            renderer.sprite = data.type.sprite;
            renderer.sortingLayerID = SortingLayer.NameToID("Units");
            renderer.sortingOrder = 0;
            obj.layer = 8;//Id of the unit layer
            obj.AddComponent<PolygonCollider2D>();
        }
    }
}
