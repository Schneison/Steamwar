using System;
using UnityEngine;
using Steamwar.Factions;

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
