using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Steamwar.Factions;
using Steamwar.Utils;

namespace Steamwar.Objects
{
    [Serializable]
    public class ObjectDataSerializable
    {
        public Vector2Int position;
        public string type;
        public int factionIndex;

        public Faction Faction { get => SessionManager.session.factions[factionIndex]; set => factionIndex = value.index; }
    }
}
