using Steamwar.Factions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Steamwar.UI
{
    public class FactionBadge : ExpandableElement
    {

        public Text nameText;
        public Image factionImage;
        public GameObject unselectedMask;
        public GameObject child;
        public bool selected;

        public override GameObject Controlled => child;

        public override Vector2 DefaultPos => Vector2.zero;

        public override Vector2 ExpandedPos => new Vector2(-8, 0);

        public override float Speed => 2;

        public void SetFaction(Faction faction)
        {
            nameText.text = faction.name;
            factionImage.color = faction.color;
        }

        public void SetSelected(bool selected)
        {
            this.selected = selected;
            unselectedMask.SetActive(!selected);
            SetExpanded(selected);
            //StartCoroutine(MoveSelected());
        }


        /*public IEnumerator MoveSelected()
        {

            RectTransform childTrans = child.GetComponent<RectTransform>();
            RectTransform trans = GetComponent<RectTransform>();
            Vector2 xDiff = new Vector2(-(childTrans.rect.x + trans.rect.width), 0);
            float time = 0;
            while (time < 1)
            {
                time += Time.deltaTime;
                Vector2 pos;
                if(selected)
                {
                    pos = Vector2.Lerp(Vector2.zero, xDiff, time * 4);
                }
                else
                {
                    pos = Vector2.Lerp(xDiff, Vector2.zero, time * 4);
                }
                childTrans.anchoredPosition = pos;
                yield return null;
            }
            /*yield return new WaitForSeconds(2.5F);
            roundMessage.color = new Color(roundMessage.color.r, roundMessage.color.g, roundMessage.color.b, 1);
            while (roundMessage.color.a > 0.0f)
            {
                roundMessage.color = new Color(roundMessage.color.r, roundMessage.color.g, roundMessage.color.b, roundMessage.color.a - (Time.deltaTime / 1.25F));
                yield return null;
            }
            roundMessage.gameObject.SetActive(false);*/
        /*}*/

    }
}