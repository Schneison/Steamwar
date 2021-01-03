using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Steamwar.Factions;
using Steamwar.Sectors;
using Steamwar.UI;
using Steamwar.Utils;
using System;

namespace Steamwar
{

    public class SessionManager : Singleton<SessionManager>
    {
        public static ServiceContainer<Registry> registryService = new ServiceContainer<Registry>(()=>new Registry(), (state) => state == LifcycleState.SESSION);

        public Camera mainCamera;
        public Grid world;
        public Tilemap ground;
        public Tilemap objects;

        internal RoundManager rounds;
        internal SectorManager sectors;
  
        public static Session session;
        public static Registry registry;

        protected override void OnInit()
        {
            registry = new Registry();
            rounds = GetComponent<RoundManager>();
            sectors = GetComponent<SectorManager>();

            StartSession();
        }
        
        /// <summary>
        /// Called at the start of the game.
        /// </summary>
        public void StartSession()
        {
            SaveManager.Load(out session);
        }

        public void Update()
        {
        }

        public void OnApplicationQuit()
        {
            SaveManager.Save(session);
        }

        public void Setup(Session session)
        {
            sectors.Load(session.activeSector);
            SessionManager.session = session;
            rounds.OnLoad(session);
        }
    }
}
