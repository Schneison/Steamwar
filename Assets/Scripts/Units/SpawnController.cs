using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using Steamwar.Renderer;

namespace Steamwar.Units
{
    public class UnitManager : IMouseListener
    {
        internal UnitType selectedUnit;

        public static GameObject CreateUnitFromData(UnitData data)
        {
            GameObject obj = CreateUnitFromType(data.type);
            obj.transform.position = data.Position;
            return obj;
        }

        public static GameObject CreateUnitFromType(UnitType type)
        {
            GameObject obj = new GameObject(type.id, new Type[] {
                typeof(SpriteRenderer),
                typeof(UnitBehaviour),
                typeof(Rigidbody2D)
            });
            obj.transform.parent = UnitController.Instance.transform;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            PolygonCollider2D collider = obj.GetComponent<PolygonCollider2D>();
            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            renderer.sprite = type.spriteBlue;
            renderer.sortingLayerID = SortingLayer.NameToID("Units");
            renderer.sortingOrder = 0;
            obj.layer = 8;//Id of the unit layer
            obj.AddComponent<PolygonCollider2D>();
            obj.AddComponent<Animator>().runtimeAnimatorController= type.animation;
            return obj;
        }

        public bool MouseDown()
        {

            if (selectedUnit == null || EventSystem.current.IsPointerOverGameObject()) {
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
            GameObject unitObj = CreateUnitFromType(selectedUnit);/*GameObject.Instantiate(UnitController.instance.unitPrefab, UnitController.instance.transform);*/
            unitObj.transform.position = pos;
            UnitBehaviour unit = unitObj.GetComponent<UnitBehaviour>();
            unit.Init(selectedUnit);
            selectedUnit = null;
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
