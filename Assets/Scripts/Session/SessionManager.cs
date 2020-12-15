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
    public class SessionManager : MonoBehaviour
    {
        public Camera mainCamera;
        public Grid world;
        public Tilemap ground;
        public Tilemap objects;

        internal RoundManager rounds;
        internal SectorManager sectors;
  
        public static Session session;
        public static SessionManager instance;
        public static Registry registry;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(gameObject);
                return;
            }
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
            //GUI: Create Faction
            string factionName = "Blue";// GUI
            uint color = 0xFF212F3D;// GUI
            Faction faction = new Faction(factionName, color);
            session.playerFaction = faction;
            session.activeFaction = faction;
            session.factions = new Faction[] { faction, new Faction("Team RED", 0xFF6A0A22) };
            session.roundFactionsSequence = session.factions;
            session.activeSector = ScriptableObjectUtility.GetAllInstances<Sector>()[0].ToData();
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
