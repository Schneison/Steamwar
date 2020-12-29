using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Steamwar.Resources;

namespace Steamwar.Factions
{
    /// <summary>
    /// A faction in the game.
    /// </summary>
    [Serializable]
    public class Faction
    {
        public string name;
        public uint color;
        public int index;
        public int roundIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of the faction that will be displayed in the game.</param>
        /// <param name="color">The color of this faction.</param>
        public Faction(string name, uint color)
        {
            this.name = name;
            this.color = color;
        }

        public bool IsPlayer
        {
            get => index == SessionManager.session.playerIndex;
        }
    }
}
