using UnityEngine;
using UnityEngine.UI;
using Steamwar.Units;
using Steamwar.Interaction;
using Steamwar.Objects;
using Steamwar.Buildings;

namespace Steamwar.UI
{
    public class ObjectInspector : MonoBehaviour, ISelectionListener
    {
        //Unit
        public Text unitName;
        public Image unitIcon;
        //State
        public Text healthText;
        public Text movmentText;
        //Faction
        public Text factionName;
        public Image factionColor;
        //Actions
        public HorizontalLayoutGroup actionContainer;

        private SelectionData? selection;
        private ActionButton[] buttons;

        public ActionButton[] ActionButtons
        {
            get
            {
                if(buttons == null)
                {
                    buttons = actionContainer.GetComponentsInChildren<ActionButton>();
                }
                return buttons;
            }
        }

        public ActionType GetActionType()
        {
            return ActionType.None;
        }

        public void OnSelection(SelectionData data, SelectionData oldData)
        {
            ObjectContainer currentObj = data.Obj;
            gameObject.SetActive(true);
            ObjectData currentData = currentObj.Data;
            ObjectType type = currentData.Type;
            unitName.text = type.displayName;
            unitIcon.sprite = type.spriteBlue;

            switch (currentObj.Kind)
            {
                case ObjectKind.BUILDING:
                    BuildingData buildingData = currentData as BuildingData;
                    healthText.text = "Helath: " + buildingData.Health;
                    movmentText.text = "";
                    break;
                case ObjectKind.UNIT:
                    UnitData unitData = currentData as UnitData;
                    healthText.text = "Helath: " + unitData.Health;
                    movmentText.text = "Movment: " + unitData.movment;
                    break;
            }

            foreach(ActionButton button in ActionButtons)
            {
                if(currentObj.HasAction(button.type)) {
                    button.Activate();
                }
                else
                {
                    button.Deactivate();
                }
            }

            factionName.text = currentData.faction.name;
            factionColor.color = currentData.faction.color;
            this.selection = data;
        }

        public static Color32 ConvertIntToColor(uint colorCode)
        {
            Color32 c = new Color32
            {
                b = (byte)((colorCode) & 0xFF),
                g = (byte)((colorCode >> 8) & 0xFF),
                r = (byte)((colorCode >> 16) & 0xFF),
                a = (byte)((colorCode >> 24) & 0xFF)
            };
            return c;
        }

        public void OnDeselection(SelectionData oldData)
        {
            if(gameObject != null)
            {
                gameObject.SetActive(false);
            }
            this.selection = null;
        }

        public bool OnInteraction(SelectionData data, InteractionContext context, out bool deselect)
        {
            deselect = false;
            // No interaction needed
            return false;
        }

        public void OnSelectionMouseMove(SelectionData data, Vector3Int cellPosition)
        {
            // No mouse move action needed
        }
    }
}
