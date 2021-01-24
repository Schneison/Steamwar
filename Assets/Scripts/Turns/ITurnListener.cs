using Steamwar.Factions;
using System.Collections;
using UnityEngine;

namespace Steamwar.Turns
{
    public interface ITurnListener 
    {
        /// <summary>
        /// Called at the start of a turn
        /// </summary>
        public void OnTurnStart();

        /// <summary>
        /// Called at the start of a turn for a faction
        /// </summary>
        public void OnFactionStart(Faction faction);

        /// <summary>
        /// Called at the end of a turn for a faction
        /// </summary>
        public void OnFactionEnd(Faction faction);

        /// <summary>
        /// Called at the end of a turn
        /// </summary>
        public void OnTurnEnd();
    }
}