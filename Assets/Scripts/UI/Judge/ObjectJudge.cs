using Steamwar.Buildings;
using Steamwar.Core;
using Steamwar.Interaction;
using Steamwar.Objects;
using System.Collections;
using UnityEngine;

namespace Steamwar.Judge
{
    public class ObjectJudge : SteamBehaviour, ISelectionListener
    {
        public ObjectEvent judgeChange;
        public ObjectEvent judgeClear;

        public ActionType GetActionType()
        {
            return ActionType.None;
        }

        public void OnDeselection(SelectionData oldData)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
            judgeClear.Invoke(oldData.Obj);
        }

        public bool OnInteraction(SelectionData data, InteractionContext context, out bool deselect)
        {
            // No interaction needed
            deselect = false;
            return false;
        }

        public void OnSelection(SelectionData data, SelectionData oldData)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
            }
            judgeChange.Invoke(data.Obj);
        }

        public void OnSelectionMouseMove(SelectionData data, Vector3Int cellPosition)
        {
            // No mouse move action needed
        }

        protected override void OnInit()
        {
            base.OnInit();
        }
    }
}