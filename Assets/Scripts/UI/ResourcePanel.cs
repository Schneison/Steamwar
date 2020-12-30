using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Steamwar.Resources;
using Steamwar.Factions;
using System;

namespace Steamwar.UI
{
    public class ResourcePanel : MonoBehaviour
    {
        public Resource resource;
        public Text currentAmount;
        public Text maxAmount;

        public void UpdateText(FactionData data)
        {
            if (!data.IsPlayer)
            {
                return;
            }
            int amount = data.Resources.moneyAmount;
            int capacity = data.Prediction.capacity.moneyAmount;
            int digets = Math.Max(3, (int)Math.Floor(Math.Log10(capacity) + 1));
            currentAmount.text = amount.ToString(digets == 3 ? "000" : "0000");
            maxAmount.text = capacity.ToString(digets == 3 ? "000" : "0000");
        }
    }
}