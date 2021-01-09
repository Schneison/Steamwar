using Steamwar;
using System.Collections;
using UnityEngine;
using Steamwar.Utils;
using Steamwar.Factions;
using Steamwar.Objects;

namespace Steamwar.UI
{
    public class UIObjectRenderer : SteamBehaviour
    {
        public ObjectContainer obj;
        SpriteRenderer[] baseRenderer;
        SpriteRenderer[] colorRenderer;
        SpriteRenderer[] renderers;

        protected override void OnInit()
        {
            base.OnInit();
            if (obj == null)
            {
                obj = GetComponentInParent<ObjectContainer>();
            }
            Transform baseTransorm = transform.Find("Base");
            Transform colorTransorm = transform.Find("Color");
            baseRenderer = baseTransorm.GetComponentsInChildren<SpriteRenderer>();
            colorRenderer = colorTransorm.GetComponentsInChildren<SpriteRenderer>();
            renderers = baseRenderer.Merge(colorRenderer);
        }

        public void OnJudgeChange(ObjectContainer container)
        {
            ObjectData data = container.Data;
            Faction faction = data.GetFaction();
            int i;
            for (i = 0; i < baseRenderer.Length; i++)
            {
                if (i >= data.Type.baseSprites.Length)
                {
                    break;
                }
                SpriteRenderer render = baseRenderer[i];
                render.sprite = data.Type.baseSprites[i];
            }
            for (i = 0; i < colorRenderer.Length; i++)
            {
                if (i >= data.Type.coloredSprites.Length)
                {
                    break;
                }
                SpriteRenderer render = colorRenderer[i];
                render.sprite = data.Type.coloredSprites[i];
                render.color = faction.color;
            }
        }

        public void OnJudgeClear(ObjectContainer container)
        {
            
        }
    }
}