using Steamwar;
using Steamwar.Factions;
using Steamwar.UI;
using Steamwar.Utils;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Steamwar.Turns
{
    public class TurnSystem : Singleton<TurnSystem>, IService, ISessionListener
    {
        public Faction[] factions;
        public int[] factionOrder;
        public int activeFaction;
        public TurnHandler activeHandler;
        public TurnHandler[] handlers;
        public int turnCount;
        public int turnMax;
        public TurnStage stage = TurnStage.PRE;
        public TurnState state;

        public TurnEvent turnStart;
        public TurnEvent turnEnd;
        public TurnFactionEvent factionStart;
        public TurnFactionEvent factionEnd;

        public void OnCreateSession(Session session)
        {
            session.factionOrder = (from faction in session.factions orderby faction.index select faction.index).ToArray();
            session.activeFaction = session.factionOrder[0];
        }

        public void OnLoadSession(Session session)
        {
            factionOrder = session.factionOrder;
            activeFaction = session.activeFaction;
            factions = session.factions;
            turnMax = session.activeSector.sector.roundsMax;
            UIElements.Instance.roundMax.text = turnMax.ToString("000");

            handlers = new TurnHandler[factions.Length];
            foreach (Faction faction in factions)
            {
                GameObject obj = new GameObject($"{faction.name.Capitalize().Replace(" ", "").Replace("_", "")}Handler");
                obj.transform.parent = transform;
                TurnHandler handler;
                if (faction.IsPlayer)
                {
                    handler = obj.AddComponent<PlayerTurnHandler>();
                }
                else
                {
                    handler = obj.AddComponent<NPFTurnHandler>();
                }
                handlers[faction.index] = handler;
            }
        }

        public void OnSaveSession(Session session)
        {
            session.activeFaction = activeFaction;
        }

        public void OnFinishLoading()
        {
        }

        public void StartTurn()
        {
            stage = TurnStage.START;
            turnCount++;
            turnStart.Invoke();
        }

        public void BetweenTurn()
        {

        }

        public void EndTurn()
        {
            stage = TurnStage.END;
            turnEnd.Invoke();
        }

        public void FixedUpdate()
        {
            if(!activeHandler?.CanProgress() ?? false)
            {
                return;
            }
            if(stage == TurnStage.END || stage == TurnStage.FACTION_END)
            {
                Faction current = FactionManager.GetFaction(activeFaction);
                FactionEnd(current);
                int nextFaction = activeFaction + 1;
                //End of
                if (nextFaction >= factionOrder.Length)
                {
                    EndTurn();
                    BetweenTurn();
                    StartTurn();
                    nextFaction = factionOrder[0];
                }
                stage = TurnStage.FACTION_START;
                Faction next = FactionManager.GetFaction(nextFaction);
                FactionStart(next);
            }
        }

        public void FactionStart(Faction faction)
        {
            factionStart.Invoke(faction);
            activeHandler = handlers[faction.index];
            activeHandler.OnFactionStart(faction);
        }

        public void FactionEnd(Faction faction)
        {
            factionEnd.Invoke(faction);
            activeHandler.OnFactionEnd(faction);
            activeHandler = null;
        }

        public IEnumerator Initialize()
        {
            yield return null;
        }

        public IEnumerator CleanUp()
        {
            yield return null;
        }

        private TurnState CreateStates()
        {

        }
    }

    [Serializable]
    public class TurnEvent : UnityEvent
    {

    }

    [Serializable]
    public class TurnFactionEvent : UnityEvent<Faction>
    {

    }
}