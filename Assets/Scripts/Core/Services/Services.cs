using Steamwar.Core;
using Steamwar.Factions;
using Steamwar.Grid;
using Steamwar.Turns;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar
{
    public static class Services
    {
        public static readonly ServiceContainer<Registry> registry = ServiceManager.Get<Registry>();
        public static readonly ServiceContainer<BoardManager> board = ServiceManager.Get<BoardManager>();
        public static readonly ServiceContainer<PropManager> props = ServiceManager.Get<PropManager>();
        public static readonly ServiceContainer<FactionManager> factions = ServiceManager.Get<FactionManager>();
        public static readonly ServiceContainer<TurnSystem> turns = ServiceManager.Get<TurnSystem>();
        public static readonly ServiceContainer<SessionManager> session = ServiceManager.Get<SessionManager>();
    }
}
