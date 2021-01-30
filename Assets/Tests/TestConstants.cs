using Steamwar;
using Steamwar.Factions;
using Steamwar.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TestConstants
{
    public static Session session = new Session()
    {
        turnCount = 20,
        turnMax = 20,
        factions = new Faction[] { new Faction(0, "enemy_0", 0), new Faction(1, "player", 0), new Faction(2, "enemy_1", 0), new Faction(3, "enemy_2", 0) },
        playerIndex = 1,
    };
    public static int playerIndex = 1;
}
