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
        public TurnState state;
        public TurnStatInstance stateInstance;

        public TurnEvent turnStart;
        public TurnEvent turnEnd;

        protected override void OnInit()
        {
            Services.turns.Create<TurnSystem>((state) => state == LifecycleState.SESSION, () => new ServiceContainer[] { Services.session, Services.board });
        }

        public void OnCreateSession(Session session)
        {
            session.turnMax = session.activeSector.sector.roundsMax;
        }

        public void OnLoadSession(Session session)
        {
            stateInstance = TurnStatInstance.CreateInstance(session);
        }

        public void OnSaveSession(Session session)
        {
            stateInstance.OnSaveSession(session);
        }

        public void OnFinishLoading()
        {
        }

        public void StartTurn()
        {
            turnStart.Invoke(stateInstance);
        }

        public void BetweenTurn()
        {

        }

        public void EndTurn()
        {
            turnEnd.Invoke(stateInstance);
        }

        public void OnButtonPressed()
        {
            if (stateInstance.IsPlayer())
            {
                stateInstance.AllowPlayerTransition();
            }
        }

        public void FixedUpdate()
        {
            if(stateInstance == null)
            {
                return;
            }
            stateInstance.Update();
        }

        public IEnumerator Initialize()
        {
            yield return null;
        }

        public IEnumerator CleanUp()
        {
            yield return null;
        }

    }

    [Serializable]
    public class TurnEvent : UnityEvent<TurnStatInstance>
    {

    }

    [Serializable]
    public class TurnFactionEvent : UnityEvent<Faction>
    {

    }
}