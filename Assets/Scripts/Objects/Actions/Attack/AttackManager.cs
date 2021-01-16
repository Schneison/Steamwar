using Steamwar;
using Steamwar.Grid;
using Steamwar.Interaction;
using Steamwar.Objects;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Attack
{
    /// <summary>
    /// Handles the attacks of units and buildings
    /// </summary>
    class AttackManager : Singleton<AttackManager>, ISelectionListener
    {
        public GameObject attackLine;
        public GameObject attackCross;
        [SerializeField]
        [MyBox.ReadOnly]
        private Vector2Int selectedCell;
        [SerializeField]
        [MyBox.ReadOnly]
        private Vector2Int mouseCell;
        [SerializeField]
        [MyBox.ReadOnly]
        private Vector2Int originCell;
        [SerializeField]
        [MyBox.ReadOnly]
        private GameObject currentLine;
        [SerializeField]
        [MyBox.ReadOnly]
        private GameObject currentCross;
        private SpriteRenderer lineRenderer;

        public ActionType GetActionType()
        {
            return ActionType.Attack;
        }

        public void OnDeselection(SelectionData oldData)
        {
            Destroy(currentLine);
            Destroy(currentCross);
        }

        public bool OnInteraction(SelectionData data, InteractionContext context, out bool deselect)
        {
            deselect = false;
            return false;
        }

        public void OnSelection(SelectionData data, SelectionData oldData)
        {
            currentLine = Instantiate(attackLine, data.Obj.gameObject.transform);
            lineRenderer = currentLine.GetComponent<SpriteRenderer>();
            currentCross = Instantiate(attackCross, data.Obj.gameObject.transform);
        }

        public void OnSelectionMouseMove(SelectionData data, Vector3Int cellPosition)
        {
            if(currentLine == null)
            {
                return;
            }
            //Vector from the object to the selected cell
            Vector3Int diff = cellPosition - data.CellPos;
            int xDiff = diff.x;
            //Distance from the object to the selected cell
            float length = Vector3Int.Distance(Vector3Int.zero, diff);
            Vector3 selectedPos = new Vector3(diff.x, diff.y);
            if (length > 6)
            {
                selectedPos = selectedPos.normalized * 6;
            }
            Vector3Int cellPos = BoardManager.WorldToCell(selectedPos);
            length = Vector3Int.Distance(Vector3Int.zero, cellPos);
            // Direction of the diff vector to calculate the angle
            Vector3 dir = new Vector3(cellPos.x, cellPos.y).normalized;
            float angle = Vector3.Angle(Vector3.up, dir);
            if(xDiff > 0)
            {
                //Angle is only from 0 to 180, so we rotate it if it is on the positive side
                angle *= -1;
            }
            //Convert the angle into a rotation
            currentLine.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            lineRenderer.size = new Vector2(1, length);

            if(currentCross == null)
            {
                return;
            }
            currentCross.transform.localPosition = new Vector3(cellPos.x, cellPos.y);
        }
    }
}
