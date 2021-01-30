using Steamwar.Grid;
using Steamwar.Interaction;
using Steamwar.Navigation;
using Steamwar.Objects;
using Steamwar.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Move
{
    public class MovementOverlay : MonoBehaviour, ISelectionListener
    {
        public GameObject selectionPrefab;

        internal Path lastPath = null;

        private GameObject selectionBoxes;
        private Collider2D selectedCollider;

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

        /// <summary>
        /// Draws debug info for the current path of the selected unit
        /// </summary>
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

        public ActionType GetActionType()
        {
            return ActionType.Move;
        }

        public void OnDeselection(SelectionData oldData)
        {
            ClearData();
        }

        private void ClearData()
        {
            lastPath = null;
            if (selectionBoxes != null)
            {
                foreach (Transform child in selectionBoxes.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public bool OnInteraction(SelectionData data, InteractionContext context, out bool deselect)
        {
            deselect = InteractionConstants.DISELECT_AFTER_MOVEMENT;
            if (lastPath == null)
            {
                return false;
            }
            data.Movement.Move(new List<PathNode>(lastPath.Skip(1)));
            ClearData();
            return true;
        }

        /// <summary>
        /// Updates the selection state of an unit.
        /// 
        /// Creates the move nodes for the unit.
        /// </summary>
        /// <param name="data"></param>
        public void OnSelectionMouseMove(SelectionData data, Vector3Int cellPosition)
        {
            ObjectContainer currentObj = data.Obj;
            if (data.AllowedToMove)
            {
                PathFinder finder = new PathFinder(BoardManager.CellToWorld(cellPosition) + new Vector3(0.5F, 0.5F), currentObj.transform.position, currentObj.gameObject, (int)currentObj.Data.turnData.moves);
                Path path = finder.FindPath();
                lastPath = path;
                // TODO: Find a better way to handle the path nodes (Don't remove / reconstruct them every time) 
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
                    foreach (PathNode node in path.Skip(1))
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

        public void OnSelection(SelectionData data, SelectionData oldData)
        {
            // Method intentionally left empty.
        }
    }
}
