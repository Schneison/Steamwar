using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Steamwar.Objects;

namespace Steamwar.Buildings
{
    public class BuildingContainer : DestroyableContainer<BuildingData, BuildingType, BuildingDataSerializable>
    {
        private SpriteRenderer spriteRenderer;

        public override ObjectKind Kind
        {
            get=>ObjectKind.BUILDING;
        }
        internal bool needInit = false;

        protected override void OnInit()
        {
            base.OnInit();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void LateUpdate()
        {
            if (needInit)
            {
                spriteRenderer.sprite = Type.spriteBlue;

                needInit = false;
            }
        }

        protected override void Construction(BuildingType type)
        {
            _data = new BuildingData
            {
                Type = type,
                Health = type.Health,
                faction = SessionManager.session.PlayerFaction
            };
            needInit = true;
        }

        public override ActionType GetAction()
        {
            return ActionType.Destroy | ActionType.Skip | ActionType.Repair;
        }
    }
}
