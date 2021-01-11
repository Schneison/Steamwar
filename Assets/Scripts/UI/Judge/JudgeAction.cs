using Assets.Scripts.UI;
using Steamwar.Factions;
using Steamwar.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using MyBox;
using Steamwar.Utils;

namespace Steamwar.UI
{
    public class JudgeAction : ExpandableElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private static readonly Color SELECTED = Faction.ConvertIntToColor(0xFFCBBC4E);
        private static readonly Vector2 DEFAULT_POS = new Vector2(25.0F, 0F);
        private static readonly Vector2 EXPANDED_POS = new Vector2(90.0F, 0F);
        /// <summary>
        /// Action type of this button
        /// </summary>
        public ActionType type;
        /// <summary>
        /// Controller obj which contains the rendering and the event listener.
        /// </summary>
        public GameObject controller;
        public Text title;
        public Image icon;
        private Color defaultFontColor;
        private Color defaultIconColor;

        public override GameObject Controlled => controller;

        public override Vector2 DefaultPos => DEFAULT_POS;

        public override Vector2 ExpandedPos => EXPANDED_POS;

        public override float Speed => 3;

        private string GetTitle(ActionType type)
        {
            return type switch
            {
                ActionType.Attack => "Attack",
                ActionType.Move => "Move",
                ActionType.Skip => "Skip",
                ActionType.Repair => "Repair",
                _ => "Missing",
            };
        }

        public Sprite GetSprite(ActionType type)
        {
            UIIcons icons = UIIcons.Instance;
            return type switch
            {
                ActionType.Attack => icons.attack,
                ActionType.Move => icons.move,
                ActionType.Skip => icons.skip,
                ActionType.Repair => icons.repair,
                _ => null,
            };
        }

        public ActionType Type
        {
            get => type; set
            {
                title.text = GetTitle(value);
                icon.sprite = GetSprite(value);
                type = value;
            }
        }


        protected override void OnInit()
        {
            base.OnInit();
            defaultFontColor = title.color;
            defaultIconColor = icon.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            icon.color = SELECTED;
            //title.color = SELECTED;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //title.color = defaultFontColor;
            icon.color = defaultIconColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ActionManager.ActivateType(type);
            //OnExpandUpdate();
        }
    }
}
