using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Steamwar.Objects;

namespace Steamwar.Buildings
{
    public class BuildingBehaviour : DestroyableObject<BuildingData, BuildingType, BuildingDataSerializable>
    {
        private SpriteRenderer spriteRenderer;

        public override ObjectKind Kind
        {
            get=>ObjectKind.BUILDING;
        }
        internal bool needInit = false;

        public override void Start()
        {
            base.Start();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void LateUpdate()
        {
            if (needInit)
            {
                spriteRenderer.sprite = data.type.spriteBlue;

                needInit = false;
            }
        }

        protected override void Construction(BuildingType type)
        {
            data = new BuildingData
            {
                type = type,
                Health = type.Health,
                faction = SessionManager.session.playerFaction
            };
            needInit = true;
        }

        public override ActionType GetAction()
        {
            return ActionType.Destroy | ActionType.Skip | ActionType.Repair;
        }
    }
}
