using UnityEngine;
using System.Collections;
using Steamwar.Utils;
using Steamwar.Factions;
using Steamwar.Grid;

namespace Steamwar.Objects
{
    public class ObjectManager : Singleton<ObjectManager>, IFactionListener
    {
        [Header("Layers")]
        public LayerMask unitLayer;
        public LayerMask groundLayer;

        public void OnFactionActivated(Faction faction) {
            foreach (ObjectContainer container in BoardManager.Board.GetObjectsFromFaction(faction.index))
            {
                container.Data.SetupTurn();
            }
        }

        public void OnFactionDeactivated(Faction faction) 
        {
            foreach(ObjectContainer container in BoardManager.Board.GetObjectsFromFaction(faction.index)) {
                container.Data.CleanupTurn();
            }
        }

        public void OnFactionUpdate(FactionData data) 
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
