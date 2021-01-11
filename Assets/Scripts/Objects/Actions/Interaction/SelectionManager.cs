using System.Collections.Generic;
using UnityEngine;
using Steamwar.Objects;
using Steamwar.Utils;
using Steamwar.Buildings;
using System.Linq;

namespace Steamwar.Interaction
{
    public class SelectionManager : Singleton<SelectionManager>, IMouseListener
    {
        public GameObject[] listenersObjects;
        public SelectionContainer listeners;

        public GameObject selectionPrefab;
        public GameObject unitSelectionPrefab;

        private GameObject unitSlectionBox;

        internal Collider2D selectedCollider;
        public SelectionData selected = SelectionData.EMPTY;

        internal Vector3Int? lastMouseCell = null;
        private bool mouseOverUI = false;

        protected override void OnInit()
        {
            List<ISelectionListener> selectionListeners = new List<ISelectionListener>();
            foreach (GameObject obj in listenersObjects)
            {
                selectionListeners.AddRange(obj.GetComponents<ISelectionListener>());
            }
            this.listeners = new SelectionContainer(selectionListeners, ()=>selected);
        }

        public static void OnUpdate(ActionType activeType)
        {
            Instance.listeners.OnActionUpdate(activeType);
        }

        /// <summary>
        /// Selects the given unit
        /// </summary>
        /// <param name="obj">Unit to be selected</param>
        public void Select(ObjectContainer obj)
        {
            SelectionData oldSelected = selected;
            Deselect();
            SelectionBehaviour selection = obj.GetComponent<SelectionBehaviour>();
            if (selection == null || !selection.Select())
            {
                return;
            }
            selected = new SelectionData(obj);
            //Change type to the default type of the object
            ActionManager.ActivateType(obj.Type.GetDefaultAction());
            unitSlectionBox = Instantiate(unitSelectionPrefab, obj.gameObject.transform);
            unitSlectionBox.transform.position -= new Vector3(0.5F, 0.5F, 0);
            selectedCollider = obj.GetComponent<Collider2D>();
            foreach (ISelectionListener listener in listeners)
            {
                listener.OnSelection(selected, oldSelected);
            }
        }

        /// <summary>
        /// Deselects the currently selected unit and deletes the selection box object.
        /// </summary>
        /// <param name="move">If cuurently selected unit should move to currently selected path.</param>
        /// <returns>True if a unit was deselected, false if no unit was selected before.</returns>
        public bool Deselect()
        {
            if(selected.IsEmpty || !selected.Selectable.Deselect())
            {
                return false;
            }

            foreach (ISelectionListener listener in listeners)
            {
                listener.OnDeselection(selected);
            }
            selected = SelectionData.EMPTY;
            Destroy(unitSlectionBox);
            selectedCollider = null;
            lastMouseCell = null;
            return true;
        }

        /// <summary>
        /// Called if the mouse button gets pressed down.
        /// </summary>
        /// <returns>True if no other handlers should be called after this one.</returns>
        public bool MouseDown()
        {
            Vector2 mousePosition = Input.mousePosition;
            // Camera camera = SessionManager.Instance.mainCamera;
            // Grid world = SessionManager.Instance.world;
            // Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);
            // Vector3Int cellPosition = world.WorldToCell(worldPos);
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, ObjectManager.Instance.unitLayer);
            if(hit.collider != null)
            {
                GameObject unitObj = hit.collider.gameObject;
                ObjectContainer unit = unitObj.GetComponent<ObjectContainer>();
                if(unit != null && !selected.IsSameObject(unit))
                {
                    Select(unit);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Called if the mouse button gets released.
        /// </summary>
        /// <returns>True if no other handlers should be called after this one.</returns>
        public bool MouseUp()
        {
            Grid world = SessionManager.Instance.world;
            Camera camera = SessionManager.Instance.mainCamera;
            ObjectContainer currentObj = selected.Obj;
            if(currentObj == null)
            {
                return false;
            }
            Vector3Int mousePos = world.LocalToCell(camera.ScreenToWorldPoint(Input.mousePosition));
            Vector3Int unitPos = world.LocalToCell(currentObj.transform.position);
            if (unitPos != mousePos)
            {
                bool deselect = false;
                foreach (ISelectionListener listener in listeners)
                {
                    if (listener.OnInteraction(selected, InteractionContext.EMPTY, out deselect))
                    {
                        break;
                    }
                }
                if(deselect)
                {
                    Deselect();
                }
            }
            return false;
        }

        /// <summary>
        /// Handles mouse moves. Checks for changes and updates the path nodes if the tile position of the mouse changed.
        /// </summary>
        /// <param name="mousePosition">Current position</param>
        /// <param name="lastMouse">Last position</param>
        /// <returns>True if no other handlers should be called after this one.</returns>
        public bool MouseMove(Vector2 mousePosition, Vector2 lastMouse)
        {
            if (InputUtil.IsPointerOverUI())
            {
                if (mouseOverUI)
                {
                    return false;
                }
                else
                {
                    foreach (ISelectionUIListener listener in listeners.OfType<ISelectionUIListener>())
                    {
                        listener.OnSelectionUIEnter(selected);
                    }
                    mouseOverUI = true;
                }
            }
            else
            {
                if (mouseOverUI)
                {
                    foreach (ISelectionUIListener listener in listeners.OfType<ISelectionUIListener>())
                    {
                        listener.OnSelectionUIExit(selected);
                    }
                    mouseOverUI = false;
                }
            }
            Camera camera = SessionManager.Instance.mainCamera;
            Grid world = SessionManager.Instance.world;
            Vector3Int cellPosition = world.WorldToCell(camera.ScreenToWorldPoint(mousePosition));
            if ((lastMouseCell == null || !lastMouseCell.Equals(cellPosition)) && !selected.IsEmpty && selected.Selectable.Selected)
            {
                foreach (ISelectionListener listener in listeners)
                {
                    listener.OnSelectionMouseMove(selected, cellPosition);
                }
            }
            return false;
        }
    }
}
