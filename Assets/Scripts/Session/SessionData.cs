using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamwar.Factions;
using Steamwar.Sectors;
using Steamwar.Objects;

namespace Steamwar
{
    [Serializable]
    public class SessionData
    {
        public Faction[] factions;
        public int playerFaction;
        /* Sector */
        public SectorData sectorData;
        /* Round */
        public int rounds;
        public int[] roundFactionsSequence;
        public int activeFaction;
        public RoundState roundState;
        /* Board */
        public BoardObjects objects;

        public Session CreateGame()
        { 
            return new Session(objects) {
                factions = factions,
                playerFaction = factions[playerFaction],

                activeSector = sectorData,

                rounds = rounds,
                roundFactionsSequence = (from factionIndex in roundFactionsSequence select factions[factionIndex]).ToArray(),
                activeFaction = factions[activeFaction],
                roundState = roundState };
        }
    }
}
