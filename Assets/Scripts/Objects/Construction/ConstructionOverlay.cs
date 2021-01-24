using Steamwar.Grid;
using Steamwar.Interaction;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Steamwar.Objects
{
    public class ConstructionOverlay : Singleton<ConstructionOverlay>, IMouseListener
    {
        public GameObject blueprint;
        public GameObject constructionLayer;

        [SerializeField]
        [MyBox.ReadOnly]
        private GameObject blueprintObj;

        private ObjectType selectedType;

        public static bool IsActive()
        {
            return Instance.selectedType != null;
        }

        public void SelectedType(ObjectType selectedType)
        {
            if (blueprintObj != null)
            {
                Destroy(blueprintObj);
            }
            this.selectedType = selectedType;
            blueprintObj = Instantiate(blueprint, transform);
            ObjectContainer unit = blueprintObj.GetComponent<ObjectContainer>();
            unit.OnConstruction(selectedType, false);
            ObjectRenderer renderer = blueprintObj.GetComponentInChildren<ObjectRenderer>();
            if (renderer != null)
            {
                renderer.UpdateTextures();
                renderer.SetTransparency(0.5F);
            }
            UpdateBlueprint();
        }

        public bool DeselectType()
        {
            if(selectedType == null)
            {
                return false;
            }
            selectedType = null;
            Destroy(blueprintObj);
            blueprintObj = null;
            EventSystem.current.SetSelectedGameObject(null);
            return true;
        }

        public void ActivateLayer()
        {
            constructionLayer.SetActive(true);
        }

        public void DeactivateLayer()
        {
            constructionLayer.SetActive(false);
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
        }

        public void OnSelection(SelectionData data, SelectionData oldData)
        {
            constructionLayer.SetActive(true);
        }

        public bool MouseDown()
        {
            return false;
        }

        public bool MouseUp()
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
            ConstructionManager.CreateOnPos(pos, selectedType);
            return true;
        }

        public bool MouseMove(Vector2 mousePosition, Vector2 lastMouse)
        {
            UpdateBlueprint();
            return false;
        }
    }
}
