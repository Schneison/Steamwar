using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamwar.Factions;
using Steamwar.Sectors;
using Steamwar.Objects;
using Steamwar.Utils;
using Steamwar.Board;

namespace Steamwar
{
    public class Session
    {
        public Faction[] factions;
        public FactionData[] factionDatas;
        public int playerIndex;
        /* Sector */
        public SectorData activeSector;
        /* Round */
        public int rounds;
        public Faction[] roundFactionsSequence;
        public Faction activeFaction;
        public RoundState roundState;
        /* Board */
        public BoardObjects objects;

        public Faction PlayerFaction { get => factions[playerIndex]; }

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
                factionDatas = factionDatas,
                playerFaction = PlayerFaction.index,

                sectorData = activeSector,

                rounds = rounds,
                roundFactionsSequence = (from faction in factions select faction.index).ToArray(),
                activeFaction = activeFaction.index,
                roundState = roundState,

                objects = objects,
            };
        }

    }
}
