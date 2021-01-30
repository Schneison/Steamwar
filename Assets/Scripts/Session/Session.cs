using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamwar.Factions;
using Steamwar.Sectors;
using Steamwar.Objects;
using Steamwar.Utils;
using Steamwar.Grid;

namespace Steamwar
{
    public class Session
    {
        public Faction[] factions;
        public int playerIndex;
        /* Sector */
        public SectorData activeSector;
        /* Round */
        public int turnCount;
        public int turnMax;
        public int[] factionOrder;
        public int activeFaction;
        /* Board */
        public BoardObjects objects;
        public Board board;

        public Faction PlayerFaction => factions[playerIndex];

        public Faction ActiveFaction => factions[activeFaction < 0 ? 0 : activeFaction];


        public Session()
        {
        }

        public Session(BoardObjects objects)
        {
            this.objects = objects.Copy();
            BoardManager.ApplyObjects(this.objects);
        }

        public SessionData CreateData()
        {
            return new SessionData()
            {
                factions = factions,
                playerFaction = PlayerFaction.index,

                sectorData = activeSector,

                turnCount = turnCount,
                turnMax = turnMax,
                factionOrder = factionOrder,
                activeFaction = activeFaction,

                objects = objects,
            };
        }

        public Session With(
            int? turnCount = null,
            int? turnMax = null,
            int[] factionOrder = null,
            int? activeFaction = null
            )
        {
            return new Session
            {
                factions = factions,
                playerIndex = playerIndex,
                activeSector = activeSector,

                turnCount = turnCount ?? this.turnCount,
                turnMax = turnMax ?? this.turnMax,
                factionOrder = factionOrder ?? this.factionOrder,
                activeFaction = activeFaction ?? this.activeFaction,


                objects = objects,
                board = board,
            };
        }
    }
}
