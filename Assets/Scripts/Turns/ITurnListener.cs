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
        public void OnTurnStart(TurnStatInstance instance);

        /// <summary>
        /// Called at the end of a turn
        /// </summary>
        public void OnTurnEnd(TurnStatInstance instance);
    }
}