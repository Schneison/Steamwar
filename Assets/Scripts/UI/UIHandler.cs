using Steamwar.Factions;
using Steamwar.Turns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Steamwar.UI
{
    public class UIHandler : MonoBehaviour
    {

        public void OnSessonLoaded(Session session)
        {
            UIElements.Instance.roundMax.text = session.turnMax.ToString("000");
        }

        public void OnTurn(TurnStatInstance instance)
        {
            UIElements.Instance.roundCounter.text = instance.turnAmount.ToString("000");
        }

        public void OnFactionStart(Faction faction)
        {
            if (!faction.IsPlayer)
            {
                return;
            }
            StartCoroutine(FadeText());
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
