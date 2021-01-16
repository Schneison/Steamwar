using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Steamwar.Renderer;
using Steamwar.Buildings;
using Steamwar.Utils;
using Steamwar.Units;
using Steamwar.Interaction;
using UnityEngine.Tilemaps;
using Steamwar.Grid;

namespace Steamwar.Objects
{
    /// <summary>
    /// Helper class to create objects.
    /// </summary>
    public class ConstructionManager : Singleton<ConstructionManager>, IMouseListener
    {
        [Header("Prefabs")]
        public GameObject unitPrefab;
        public GameObject buildingPrefab;
        [Header("Object Containers")]
        public GameObject unitContainer;
        public GameObject buildingContainer;

        private ObjectType selectedType;
        [MyBox.ReadOnly]
        private GameObject blueprintObj;

        public static GameObject CreateObjectFromData(ObjectData data)
        {
            GameObject obj = CreateBaseObject(data.Type);
            if (obj != null)
            {
                obj.transform.position = data.Position;
            }
            return obj;
        }

        private GameObject CreateFromPrefab(ObjectType type)
        {
            switch (type.kind)
            {
                case ObjectKind.UNIT:
                    return Instantiate(unitPrefab, unitContainer.transform);
                case ObjectKind.BUILDING:
                    return Instantiate(buildingPrefab, buildingContainer.transform);
                default:
                    GameObject obj = new GameObject(type.id, new Type[] { typeof(SpriteRenderer), typeof(Rigidbody2D) });
                    obj.transform.parent = Instance.transform;
                    SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                    Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
                    rigidbody.bodyType = RigidbodyType2D.Kinematic;
                    renderer.sprite = type.spriteBlue;
                    renderer.sortingLayerID = type.kind == ObjectKind.UNIT ? SortingLayer.NameToID("Units") : SortingLayer.NameToID("Objects");
                    renderer.sortingOrder = 0;
                    obj.layer = 8;//Id of the unit layer
                    obj.AddComponent<PolygonCollider2D>();
                    return obj;
            }
        }

        public static GameObject CreateBaseObject(ObjectType type)
        {
            GameObject objInstance = Instance.CreateFromPrefab(type);
            ObjectContainer obj = objInstance.GetComponent<ObjectContainer>();
            if (obj != null)
            {
                obj.OnPrefabInit();
                AddElement(obj, type);
            }
            return objInstance;
        }

        public static void AddElement(MonoBehaviour obj, ObjectType type)
        {
            if (type.elementPrefab != null)
            {
                GameObject element = Instantiate(type.elementPrefab);
                element.name = "Element";
                element.transform.parent = obj.transform;
            }
        }

        public bool MouseDown()
        {

            if (selectedType == null || InputUtil.IsPointerOverUI())
            {
                return false;
            }
            Vector2 pos = BoardManager.ScreenToUnit(Input.mousePosition);
            RaycastHit2D ray = Physics2D.BoxCast(pos, new Vector2(0.5F, 0.5F), 0.0F, Vector2.zero, ObjectManager.Instance.groundLayer);
            if (ray.collider != null)
            {
                return false;
            }
            Destroy(blueprintObj);
            blueprintObj = null;
            GameObject unitObj = CreateBaseObject(selectedType);
            unitObj.transform.position = pos;
            ObjectContainer unit = unitObj.GetComponent<ObjectContainer>();
            unit.OnConstruction(selectedType);
            selectedType = null;
            return true;
        }

        public void SetSelectedType(ObjectType selectedType)
        {
            this.selectedType = selectedType;
            blueprintObj = CreateBaseObject(selectedType);
            ObjectContainer unit = blueprintObj.GetComponent<ObjectContainer>();
            Destroy(unit.GetComponent<Collider2D>());
            unit.OnConstruction(selectedType);
            UpdateBlueprint();
        }

        public bool MouseUp()
        {
            return false;
        }

        public bool MouseMove(Vector2 mousePosition, Vector2 lastMouse)
        {
            UpdateBlueprint();
            return false;
        }

        private void UpdateBlueprint()
        {
            if (blueprintObj == null)
            {
                return;
            }
            Vector3Int cellPos = BoardManager.ScreenToCell(Input.mousePosition);
            Vector2 pos = BoardManager.ScreenToUnit(Input.mousePosition);
            var tile = BoardManager.GetTile(cellPos);
            blueprintObj.transform.position = pos;
            blueprintObj.transform.parent = transform;
            ObjectRenderer renderer = blueprintObj.GetComponentInChildren<ObjectRenderer>();
            if (renderer != null)
            {
                renderer.SetTransparency(0.5F);
            }
        }
    }
}
