using Steamwar.Core;
using Steamwar.Supplies;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Factions
{
    [Serializable]
    [RequireComponent(typeof(SupplyContainer))]
    public class FactionData : SteamBehaviour
    {
        public readonly int factionIndex;
        private SupplyContainer _resources;

        public FactionData(int factionIndex)
        {
            this.factionIndex = factionIndex;
        }

        protected override void OnInit()
        {
            _resources = GetComponent<SupplyContainer>();
        }

        public bool IsPlayer
        {
            get => FactionManager.IsPlayerFaction(factionIndex);
        }

        public SupplyContainer Resources 
        { 
            get => _resources;
        }

    }
}
