using Steamwar.Factions;
using Steamwar.Objects;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Steamwar.UI
{
    public class JudgeName : SteamBehaviour, IJudgeListener
    {
        public Text title;
        public Image factionColor;

        public void OnJudgeChange(ObjectContainer container)
        {
            title.text = container.Type.displayName;
            factionColor.color = container.Data.GetFaction().color;
        }

        public void OnJudgeClear(ObjectContainer container)
        {
            title.text = "Cleared";
            factionColor.color = Color.magenta;
        }

        protected override void OnInit()
        {
            base.OnInit();
        }
    }
}