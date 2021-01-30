using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Steamwar.Factions;
using Steamwar.Sectors;
using Steamwar.UI;
using Steamwar.Utils;
using System;
using Steamwar.Core;
using UnityEngine.Events;

namespace Steamwar
{

    public class SessionManager : Singleton<SessionManager>, IFinishService
    {
        public delegate Session SessionTransformer(Session session);

        public SessionEvent sessionLoaded;
        public SessionEvent sessionCreated;
        public SessionEvent sessionSaved;
        public Camera mainCamera;

        internal RoundManager rounds;
        internal SectorManager sectors;
  
        public static Session session;
        public static Registry registry;

        protected override void OnInit()
        {
            registry = new Registry();
            rounds = GetComponent<RoundManager>();
            sectors = GetComponent<SectorManager>();
            Services.registry.Create(()=>new Registry(), (state) => state == LifecycleState.SESSION);
            Services.session.Create<SessionManager>((state) => state == LifecycleState.SESSION, () => new ServiceContainer[] { Services.registry });
            StartSession();
        }
        
        /// <summary>
        /// Called at the start of the game.
        /// </summary>
        public void StartSession()
        {
            GameManager.Instance.StartSession();
            /*SaveManager.Load(out session);
            GameManager.Instance.StartSession();
            sessionCreated.Invoke(session);
            sessionLoaded.Invoke(session);*/
            //factionChanged.Invoke(session);
        }

        public void OnApplicationQuit()
        {
            SaveManager.Save(session);
        }

        public void Setup(Session session)
        {
            sectors.Load(session.activeSector);
            SessionManager.session = session;
        }

        public static void UpdateSession(SessionTransformer transformer)
        {
            Session newSession = transformer(session);
        }

        public IEnumerator Initialize()
        {
            SaveManager.Load(out session);
            sessionCreated.Invoke(session);
            yield return null;
        }

        public IEnumerator Finish()
        {
            sessionLoaded.Invoke(session);
            yield return null;
        }

        public IEnumerator CleanUp()
        {
            yield return null;
        }
    }

    [Serializable]
    public class SessionEvent : UnityEvent<Session>
    {

    }
}
