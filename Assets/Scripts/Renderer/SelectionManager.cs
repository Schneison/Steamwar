using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Steamwar.Units;
using Steamwar.UI;

namespace Steamwar.Renderer
{
    public class SelectionManager : MonoBehaviour, IMouseListener
    {
        public GameObject selectionPrefab;
        public GameObject unitSelectionPrefab;
        public UnitInfo unitInfo;
        public LayerMask unitLayer;
        public LayerMask groundLayer;
        private GameObject selectionBoxes;
        private GameObject unitSlectionBox;
        private Vector3Int? lastMouse = null;
        internal UnitBehaviour selectedUnit;
        internal Collider2D selectedCollider;
        internal Path lastPath = null;
        Gradient gradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        private void Awake()
        {
            selectionBoxes = new GameObject();
            selectionBoxes.transform.parent = transform;
            selectionBoxes.name = "SelectionBoxes";
            gradient = new Gradient();
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.red;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.blue;
            colorKey[1].time = 1.0f;

            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;
            gradient.SetKeys(colorKey, alphaKey);
        }

        private void OnDrawGizmos()
        {
            /*if(lastPath == null)
            {
                return;
            }
            int maxF = lastPath.First().priority;
            foreach(PathNode point in lastPath.nodes)
            {
                if(maxF < point.priority) {
                    maxF = point.priority;
                }
            }
            foreach (PathNode point  in lastPath.nodes)
            {
                Gizmos.color = lastPath.Contains(point) ? lastPath.nodes[0] == point ? Color.yellow : lastPath.destination == point ? Color.magenta : Color.green :  gradient.Evaluate((float)point.priority / (float)(maxF));
                Gizmos.DrawCube(point.position, new Vector2(0.5F, 0.5F));
                if (point.previous != null)
                {
                    Gizmos.DrawLine(point.position, point.previous.position);
                }
            }*/
        }

        public void Select(UnitBehaviour unit)
        {
            Deselect(false);
            unitSlectionBox = Instantiate(unitSelectionPrefab, unit.gameObject.transform);
            unitSlectionBox.transform.position -= new Vector3(0.5F, 0.5F, 0);
            selectedUnit = unit;
            selectedUnit.selected = true;
            selectedCollider = selectedUnit.GetComponent<Collider2D>();
            unitInfo.Select(unit);
        }

        public bool Deselect(bool move)
        {
            if(lastPath == null && selectedUnit == null)
            {
                return false;
            }
            if (move && lastPath != null)
            {
                selectedUnit.Move(new List<PathNode>(lastPath.Skip(1)));
            }
            lastPath = null;
            Destroy(unitSlectionBox);
            if (selectionBoxes != null)
            {
                foreach (Transform child in selectionBoxes.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            lastMouse = null;
            if(selectedUnit != null)
            {
                selectedUnit.selected = false;
                selectedUnit = null;
            }
            selectedCollider = null;
            unitInfo.Deselect(selectedUnit);
            return true;
        }

        public void UpdateSelection(UnitBehaviour unit)
        {
            Vector2 mousePosition = Input.mousePosition;
            Camera camera = SessionManager.instance.mainCamera;
            Grid world = SessionManager.instance.world;
            Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);
            Vector3Int cellPosition = world.WorldToCell(worldPos);
            if (lastMouse == null || !lastMouse.Equals(cellPosition))
            {
                PathFinder finder = new PathFinder(world.CellToWorld(cellPosition) + new Vector3(0.5F, 0.5F), new Vector2(unit.transform.position.x, unit.transform.position.y), unit.gameObject);
                Path path = finder.FindPath();
                lastMouse = cellPosition;
                lastPath = path;
                
                if (selectionBoxes != null)
                {
                     foreach (Transform child in selectionBoxes.transform)
                     {
                         Destroy(child.gameObject);
                     }
                }
                if (path.length > 0)
                 {
                    int index = 1;
                    foreach(PathNode node in path.Skip(1))
                    {
                        GameObject obj = Instantiate(selectionPrefab, selectionBoxes.transform);
                        obj.transform.name = "Selection(" + index + ")";
                        obj.transform.position = node.position;
                        obj.GetComponentInChildren<TextMesh>().text = index.ToString();
                        index++;
                    }
                 }
            }
        }

        public bool CanMoveTo(Vector2 from , Vector2 to)
        {
            RaycastHit2D groundHit = Physics2D.Linecast(from, to, groundLayer);
            if(groundHit.collider != null)
            {
                return false;
            }
            selectedCollider.enabled = false;
            RaycastHit2D unitHit = Physics2D.Linecast(from, to, unitLayer);
            selectedCollider.enabled = true;
            return unitHit.collider == null;
        }

        public bool MouseDown()
        {
            Vector2 mousePosition = Input.mousePosition;
            Grid world = SessionManager.instance.world;
            Camera camera = SessionManager.instance.mainCamera;
            Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);
            Vector3Int cellPosition = world.WorldToCell(worldPos);
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, unitLayer);
            if(hit.collider != null)
            {
                GameObject unitObj = hit.collider.gameObject;
                UnitBehaviour unit = unitObj.GetComponent<UnitBehaviour>();
                if(unit != null && unit.data.faction == SessionManager.session.playerFaction && unit != selectedUnit)
                {
                    Select(unit);
                    return true;
                }
            }
            return false;
        }

        public bool MouseUp()
        {
            Grid world = SessionManager.instance.world;
            Camera camera = SessionManager.instance.mainCamera;
            UnitBehaviour unit = selectedUnit;
            if (unit != null
                && world.LocalToCell(unit.transform.position) != world.LocalToCell(camera.ScreenToWorldPoint(Input.mousePosition)))
            {
                Deselect(true);
            }
            return false;
        }

        public bool MouseMove(Vector2 mousePosition, Vector2 lastMouse)
        {
            if (selectedUnit != null && selectedUnit.selected && !selectedUnit.moves)
            {
                UpdateSelection(selectedUnit);
            }
            return false;
        }
    }
}
