using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamwar.Factions;
using Steamwar.Sectors;
using Steamwar.Objects;
using Steamwar.Grid;

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
        public int turnCount;
        public int turnMax;
        public int[] factionOrder;
        public int activeFaction;
        /* Board */
        public BoardObjects objects;

        public Session CreateGame()
        {
            return new Session(objects) {
                factions = factions,
                playerIndex = playerFaction,

                activeSector = sectorData,

                turnCount = turnCount,
                turnMax = turnMax,
                factionOrder = factionOrder,
                activeFaction = activeFaction,
                 };
        }
    }
}
