using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Factions
{
    /// <summary>
    /// Method that gets called every time the faction data get an update. To recieve this call add the object to the event listener of 'EventManager.Instance.factionUpdate'.
    /// </summary>
    public interface IFactionListener
    {
        void OnFactionUpdate(FactionData data);

        void OnFactionActivated(Faction faction);

        void OnFactionDeactivated(Faction faction);
    }
}
