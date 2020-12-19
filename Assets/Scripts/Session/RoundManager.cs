using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamwar.Factions;
using UnityEngine;
using UnityEngine.UI;

namespace Steamwar
{
    public class RoundManager : MonoBehaviour
    {
        public Text roundText;
        public Text maxRoundText;

        public Text roundMessage;

        public void Init()
        {
            maxRoundText.text = SessionManager.session.activeSector.sector.roundsMax.ToString("000");
        }

        public void OnLoad(Session game)
        {
            if (game.activeFaction == game.playerFaction)
            {
                roundMessage.gameObject.SetActive(true);
                StartCoroutine(FadeText());
            }
        }

        public void EndRound()
        {
            ProgressRound();
        }

        public void Start()
        {
            StartRound();
        }

        public void StartRound()
        {
            Session game = SessionManager.session;
            game.rounds += 1;
            roundText.text = game.rounds.ToString("000");
            ProgressRound();
        }

        public void NextState()
        {
            if(Enum.IsDefined(typeof(RoundState), (int)SessionManager.session.roundState + 1))
            {
                SessionManager.session.roundState += 1;
            }
            else
            {
                SessionManager.session.roundState = RoundState.PRE;
            }
        }

        public void ProgressRound()
        {
            Session session = SessionManager.session;
            RoundState state = session.roundState;
            switch (state)
            {
                case RoundState.PRE:
                    ProgressRound(session.roundFactionsSequence[0]);
                    break;
                case RoundState.FACTIONS:
                    int factionIndex = Array.IndexOf(session.factions, session.activeFaction) + 1;
                    Faction oldFaction = session.activeFaction;
                    int newIndex = factionIndex + 1;
                    if (newIndex < session.roundFactionsSequence.Length)
                    {
                        Faction newFaction = session.roundFactionsSequence[newIndex];
                        if(newFaction == session.playerFaction)
                        {
                            roundMessage.gameObject.SetActive(true);
                            StartCoroutine(FadeText());
                        }
                        session.activeFaction = newFaction;
                    }
                    else
                    {
                        EndRound();
                    }
                    break;
                case RoundState.END:
                    break;
            }
        }

        public void ProgressRound(Faction faction)
        {
            Session game = SessionManager.session;
            game.activeFaction = faction;
        }

        public System.Collections.IEnumerator FadeText()
        {
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

    [Serializable]
    public enum RoundState
    {
        PRE,
        FACTIONS,
        END
    }
}
