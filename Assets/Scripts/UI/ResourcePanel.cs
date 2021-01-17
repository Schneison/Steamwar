using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Steamwar.Supplies;
using Steamwar.Factions;
using System;

namespace Steamwar.UI
{
    public class ResourcePanel : MonoBehaviour
    {
        public Supply resource;
        public Text currentAmount;
        public Text maxAmount;

        public void UpdateText(SupplyContainer container)
        {
            if (!container.IsPlayer)
            {
                return;
            }
            int amount = container[resource];
            int capacity = container.Capacity[resource];
            int digets = Math.Max(3, (int)Math.Floor(Math.Log10(capacity) + 1));
            currentAmount.text = amount.ToString(digets == 3 ? "000" : "0000");
            maxAmount.text = capacity.ToString(digets == 3 ? "000" : "0000");
        }
    }
}