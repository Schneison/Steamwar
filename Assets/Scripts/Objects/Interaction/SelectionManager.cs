using System.Collections.Generic;
using UnityEngine;
using Steamwar.Objects;

namespace Steamwar.Interaction
{
    public class SelectionManager : MonoBehaviour, IMouseListener
    {
        public GameObject[] listenersObjects;
        public ISelectionListener[] listeners;

        public GameObject selectionPrefab;
        public GameObject unitSelectionPrefab;

        public LayerMask unitLayer;
        public LayerMask groundLayer;
        private GameObject unitSlectionBox;

        internal Collider2D selectedCollider;
        public SelectionData selected = SelectionData.EMPTY;


        void Start()
        {
            List<ISelectionListener> selectionListeners = new List<ISelectionListener>();
            foreach (GameObject obj in listenersObjects)
            {
                selectionListeners.AddRange(obj.GetComponents<ISelectionListener>());
            }
            this.listeners = selectionListeners.ToArray();
        }

        /// <summary>
        /// Selects the given unit
        /// </summary>
        /// <param name="obj">Unit to be selected</param>
        public void Select(ObjectBehaviour obj)
        {
            SelectionData oldSelected = selected;
            Deselect();
            if (!obj.Select())
            {
                return;
            }
            selected = new SelectionData(obj);
            unitSlectionBox = Instantiate(unitSelectionPrefab, obj.gameObject.transform);
            unitSlectionBox.transform.position -= new Vector3(0.5F, 0.5F, 0);
            selectedCollider = obj.GetComponent<Collider2D>();
            foreach (ISelectionListener listener in listeners)
            {
                if (listener != null)
                {
                    listener.OnSelection(selected, oldSelected);
                }
            }
        }

        /// <summary>
        /// Deselects the currently selected unit and deletes the selection box object.
        /// </summary>
        /// <param name="move">If cuurently selected unit should move to currently selected path.</param>
        /// <returns>True if a unit was deselected, false if no unit was selected before.</returns>
        public bool Deselect()
        {
            if(selected.IsEmpty || !selected.Obj.Deselect())
            {
                return false;
            }

            foreach (ISelectionListener listener in listeners)
            {
                if (listener != null)
                {
                    listener.OnDeselection(selected);
                }
            }
            selected = SelectionData.EMPTY;
            Destroy(unitSlectionBox);
            selectedCollider = null;
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

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, unitLayer);
            if(hit.collider != null)
            {
                GameObject unitObj = hit.collider.gameObject;
                ObjectBehaviour unit = unitObj.GetComponent<ObjectBehaviour>();
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
            ObjectBehaviour currentObj = selected.Obj;
            if (currentObj != null &&
                world.LocalToCell(currentObj.transform.position) != world.LocalToCell(camera.ScreenToWorldPoint(Input.mousePosition)))
            {
                bool deselect = false;
                foreach (ISelectionListener listener in listeners)
                {
                    if (listener != null && listener.OnInteraction(selected, InteractionContext.EMPTY, out deselect))
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
            if (!selected.IsEmpty && selected.Obj.Selected && selected.Obj.CanMove)
            {
                foreach (ISelectionListener listener in listeners)
                {
                    if (listener != null)
                    {
                        listener.OnSelectionMouseMove(selected);
                    }
                }
            }
            return false;
        }
    }
}
