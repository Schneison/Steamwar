using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Factions
{
    public interface IFactionProvider
    {
        public int FactionIndex
        {
            get;
            set;
        }
    }

    public static class FactionProviderExtension
    {
        public static Faction GetFaction(this IFactionProvider source)
        {
            return FactionManager.GetFaction(source.FactionIndex);
        }

        public static bool HasFaction(this IFactionProvider source, Faction faction)
        {
            return HasFaction(source, faction.index);
        }

        public static bool HasFaction(this IFactionProvider source, int index)
        {
            return source.FactionIndex == index;
        }

        public static bool HasPlayerFaction(this IFactionProvider source)
        {
            return FactionManager.IsPlayerFaction(source.FactionIndex);
        }
    }
}
