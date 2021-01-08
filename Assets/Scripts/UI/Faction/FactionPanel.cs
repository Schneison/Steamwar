using Steamwar.Core;
using Steamwar.Factions;
using System.Collections;
using UnityEngine;

namespace Steamwar.UI
{
    public class FactionPanel : SteamBehaviour
    {
        private FactionBadge[] badges;
        private FactionBadge activeBadge;

        public FactionBadge badgePrefab;

        public void SessionLoaded(Session session)
        {
            for(int i = 0;i < transform.childCount;i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            badges = new FactionBadge[session.factions.Length];
            foreach (Faction faction in session.factions)
            {
                FactionBadge badge = Instantiate(badgePrefab, transform);
                badge.name = $"Faction({faction.name})";
                badge.SetFaction(faction);
                badges[faction.index] = badge;
            }
        }

        public void OnFactionActivated(Session session)
        {
            if(activeBadge != null)
            {
                activeBadge.SetSelected(false);
            }
            activeBadge = badges[session.activeFaction.index];
            activeBadge.SetSelected(true);
        }

    }
}