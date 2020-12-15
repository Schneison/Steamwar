using UnityEngine;
using UnityEngine.UI;
using Steamwar.Units;
using Steamwar.Interaction;
using Steamwar.Objects;
using Steamwar.Buildings;

namespace Steamwar.UI
{
    public class UnitInfo : MonoBehaviour, ISelectionListener
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

        private SelectionData? selection;

        public void OnSelection(SelectionData data, SelectionData oldData)
        {
            ObjectBehaviour currentObj = data.Obj;
            gameObject.SetActive(true);
            ObjectData currentData = currentObj.Data;
            ObjectType type = currentData.Type;
            unitName.text = type.displayName;
            unitIcon.sprite = type.spriteBlue;

            switch (currentObj.Kind)
            {
                case ObjectKind.BUILDING:
                    BuildingData buildingData = currentData as BuildingData;
                    healthText.text = "Helath: " + buildingData.health;
                    movmentText.text = "";
                    break;
                case ObjectKind.UNIT:
                    UnitData unitData = currentData as UnitData;
                    healthText.text = "Helath: " + unitData.health;
                    movmentText.text = "Movment: " + unitData.movment;
                    break;
            }

            factionName.text = currentData.faction.name;
            factionColor.color = ConvertIntToColor(currentData.faction.color);
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

        public void OnSelectionMouseMove(SelectionData data)
        {
            // No mouse move action needed
        }
    }
}
