using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Steamwar.Renderer;
using Steamwar.Buildings;
using Steamwar.Utils;
using Steamwar.Units;

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

        internal ObjectType selectedType;

        public static GameObject CreateObjectFromData(ObjectData data)
        {
            GameObject obj = CreateObjectFromType(data.Type);
            if (obj != null)
            {
                obj.transform.position = data.Position;
            }
            return obj;
        }

        public static GameObject CreateObjectFromType(ObjectType type)
        {
            if (type is UnitType)
            {
                return CreateUnitFromType(type as UnitType);
            }else if(type is BuildingType)
            {
                return CreateBuildingFromType(type as BuildingType);
            }
            return null;
        }

        private GameObject CreateFromPrefab(ObjectType type)
        {
            if (type is UnitType)
            {
                return Instantiate(unitPrefab, unitContainer.transform);
            }
            else if (type is BuildingType)
            {
                return Instantiate(buildingPrefab, buildingContainer.transform);
            }
            GameObject obj = new GameObject(type.id, new Type[] {
                typeof(SpriteRenderer),
                typeof(Rigidbody2D)
            });
            obj.transform.parent = Instance.transform;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            renderer.sprite = type.spriteBlue;
            renderer.sortingLayerID = type is UnitType ? SortingLayer.NameToID("Units") : SortingLayer.NameToID("Objects");
            renderer.sortingOrder = 0;
            obj.layer = 8;//Id of the unit layer
            obj.AddComponent<PolygonCollider2D>();
            return obj;
        }

        public static GameObject CreateBaseObject(ObjectType type)
        {
            GameObject objInstance = Instance.CreateFromPrefab(type);
            ObjectBehaviour obj = objInstance.GetComponent<ObjectBehaviour>();
            if(obj != null)
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

        public static GameObject CreateUnitFromType(UnitType type)
        {
            GameObject obj = CreateBaseObject(type);
            SpriteRenderer renderer = obj.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = type.spriteBlue;
            renderer.gameObject.AddComponent<Animator>().runtimeAnimatorController= type.animation;
            return obj;
        }

        public static GameObject CreateBuildingFromType(BuildingType type)
        {
            GameObject obj = CreateBaseObject(type);
            return obj;
        }

        public bool MouseDown()
        {

            if (selectedType == null || EventSystem.current.IsPointerOverGameObject()) {
                return false;
            }
            Camera camera = SessionManager.Instance.mainCamera;
            Grid world = SessionManager.Instance.world;
            Vector2 mousePosition = Input.mousePosition;
            Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);
            Vector3Int cellPos = world.WorldToCell(worldPos);
            Vector2 pos = (Vector2)world.CellToWorld(cellPos) + new Vector2(0.5F, 0.5F);
            RaycastHit2D ray = Physics2D.BoxCast(pos, new Vector2(0.5F, 0.5F), 0.0F, Vector2.zero, SessionRenderer.Instance.selection.groundLayer);
            if(ray.collider != null)
            {
                return false;
            }
            GameObject unitObj = CreateObjectFromType(selectedType);
            unitObj.transform.position = pos;
            ObjectBehaviour unit = unitObj.GetComponent<ObjectBehaviour>();
            unit.OnConstruction(selectedType);
            selectedType = null;
            return true;
        }

        public bool MouseUp()
        {
            return false;
        }

        public bool MouseMove(Vector2 mousePosition, Vector2 lastMouse)
        {
            return false;
        }
    }
}
