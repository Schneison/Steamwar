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

namespace Steamwar
{

    public class SessionManager : Singleton<SessionManager>
    {
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

            StartSession();
        }
        
        /// <summary>
        /// Called at the start of the game.
        /// </summary>
        public void StartSession()
        {
            SaveManager.Load(out session);
            GameManager.Instance.StartSession();
            EventManager.Instance.sessionLoaded.Invoke(session);
            EventManager.Instance.factionChanged.Invoke(session);
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
    }

    [Flags]
    public enum SessionState
    {
        NONE,
        BOARD_CREATED = 1,
        OBJECTS_CONSTRUCTED = 2,
        LOADING = 4,
        SESSION = 8,
        SHUTTTING_DOWN = 16
    }
}
