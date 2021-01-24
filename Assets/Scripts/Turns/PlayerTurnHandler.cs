using Steamwar.Factions;
using Steamwar.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Steamwar.Turns
{
    public class PlayerTurnHandler : SteamBehaviour, TurnHandler
    {

        [SerializeField]
        private bool nextRound;

        protected override void OnSpawn()
        {
            //roundText = UIElements.Instance.roundCounter;
            //maxRoundText = UIElements.Instance.roundMax;
           // roundMessage = UIElements.Instance.centerMessage;
        }

        public bool CanProgress()
        {
            return nextRound;
        }

        public void OnFactionEnd(Faction faction)
        {
            if (!faction.IsPlayer)
            {
                return;
            }

        }

        public void OnFactionStart(Faction faction)
        {
            if (!faction.IsPlayer)
            {
                return;
            }
            StartCoroutine(FadeText());

        }

        public void OnTurnEnd()
        {

        }

        public void OnTurnStart()
        {

        }

        public IEnumerator FadeText()
        {
            Text roundMessage = UIElements.Instance.roundCounter;
            roundMessage.gameObject.SetActive(true);
            roundMessage.color = new Color(roundMessage.color.r, roundMessage.color.g, roundMessage.color.b, 0);
            while (roundMessage.color.a < 1.0f)
            {
                roundMessage.color = new Color(roundMessage.color.r, roundMessage.color.g, roundMessage.color.b, roundMessage.color.a + (Time.deltaTime / 1.25F));
                yield return null;
            }
            yield return new WaitForSeconds(2.5F);
            roundMessage.color = new Color(roundMessage.color.r, roundMessage.color.g, roundMessage.color.b, 1);
            while (roundMessage.color.a > 0.0f)
            {
                roundMessage.color = new Color(roundMessage.color.r, roundMessage.color.g, roundMessage.color.b, roundMessage.color.a - (Time.deltaTime / 1.25F));
                yield return null;
            }
            roundMessage.gameObject.SetActive(false);
        }
    }
}