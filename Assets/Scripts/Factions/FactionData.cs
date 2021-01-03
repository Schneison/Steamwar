using Steamwar.Core;
using Steamwar.Resources;
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
    [RequireComponent(typeof(ResourceContainer))]
    public class FactionData : SteamBehaviour
    {
        public readonly int factionIndex;
        private ResourceContainer _resources;

        public FactionData(int factionIndex)
        {
            this.factionIndex = factionIndex;
        }

        protected override void OnInit()
        {
            _resources = GetComponent<ResourceContainer>();
        }

        public bool IsPlayer
        {
            get => FactionManager.IsPlayerFaction(factionIndex);
        }

        public ResourceContainer Resources 
        { 
            get => _resources;
        }

    }
}
