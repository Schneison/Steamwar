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
        public static readonly Faction None = new Faction(-1, "missing", 0xFF251905);
        
                public static Color32 ConvertIntToColor(uint colorCode)
        {
            Color32 c = new Color32
            {
                b = (byte)((colorCode) & 0xFF),
                g = (byte)((colorCode >> 8) & 0xFF),
                r = (byte)((colorCode >> 16) & 0xFF),
                a = (byte)((colorCode >> 24) & 0xFF)
            };
            return c;
        }

        public string name;
        public Color color;
        public int index;
        public int roundIndex;

        public Faction(string name, uint color) : this(name, ConvertIntToColor(color))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of the faction that will be displayed in the game.</param>
        /// <param name="color">The color of this faction.</param>
        public Faction(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        public Faction(int index, string name, uint color)
        {
            this.index = index;
            this.name = name;
            this.color = ConvertIntToColor(color);
        }

        public bool Exists
        {
            get => index >= 0;
        }

        public bool IsPlayer
        {
            get => FactionManager.IsPlayerFaction(index);
        }

        public FactionData Data
        {
            get => FactionManager.GetData(index);
        }
    }
}
