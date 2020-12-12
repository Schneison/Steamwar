using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Steamwar.Units;

namespace Steamwar.UI
{
    public class UnitInfo : MonoBehaviour
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

        private UnitBehaviour unit;

        public void Select(UnitBehaviour unit)
        {
            gameObject.SetActive(true);
            UnitData data = unit.data;
            UnitType type = data.type;
            unitName.text = type.displayName;
            unitIcon.sprite = type.spriteBlue;

            healthText.text = "Helath: " + data.health;
            movmentText.text = "Movment: " + data.health;

            factionName.text = data.faction.name;
            factionColor.color = ConvertAndroidColor(data.faction.color);
            this.unit = unit;
        }

        public static Color32 ConvertAndroidColor(int aCol)
        {
            Color32 c = new Color32
            {
                b = (byte)((aCol) & 0xFF),
                g = (byte)((aCol >> 8) & 0xFF),
                r = (byte)((aCol >> 16) & 0xFF),
                a = (byte)((aCol >> 24) & 0xFF)
            };
            return c;
        }

        public void Deselect(UnitBehaviour unit)
        {
            if(gameObject != null)
            {
                gameObject.SetActive(false);
            }
            this.unit = null;
        }
    }
}
