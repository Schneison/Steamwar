using UnityEngine;
using Steamwar.Factions;
using System.Collections.Generic;
using System;
using Steamwar.Utils;
using Steamwar.Objects;
using Steamwar.Core;
using Steamwar.Move;

namespace Steamwar.Units {

    public class UnitContainer : DestroyableContainer<UnitData, UnitType, UnitDataSerializable>
    {
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        internal bool needInit = false;
        internal Direction facingDirection;

        protected override void OnInit()
        {
            base.OnInit();
            animator = GetComponentInChildren<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public override bool CanMove { get => IsMovable && !Moves; }

        public override bool Moves { get; set; }

        public override bool IsMovable { get => true; }

        public virtual void LateUpdate()
        {
            if(needInit)
            {
                spriteRenderer.sprite = Type.spriteBlue;
                
                needInit = false;
            }
        }

        protected override void Construction(UnitType type)
        {
            _data = new UnitData
            {
                Type = type,
                Health = type.Health,
                faction = SessionManager.session.PlayerFaction
            };
            needInit = true;
        }

        public override ObjectKind Kind
        {
            get=> ObjectKind.UNIT;
        }

        /* Movment */
        public void Move(IEnumerable<PathNode> path)
        {
            MovementManager.Move(this, path);
        }

        /* Living */
        public virtual void Damage(UnitContainer attackedUnit)
        {
            float damage = GetDamage(attackedUnit);
            float health = GetHealth();
            if (damage > 0)
            {
                health -= damage;
            }
            if(health <= 0)
            {
                OnDeath(attackedUnit);
                attackedUnit.OnKill(this);
            }
        }

        public virtual float GetHealth()
        {
            return Data.Health;
        }

        public virtual float GetDamage(UnitContainer attackedUnit)
        {
            return Type.damage;
        }

        public void UpdateAnimation()
        {
            if(animator == null)
            {
                return;
            }
            animator.SetBool("moving", Moves && (facingDirection == Direction.LEFT || facingDirection == Direction.RIGHT));
            animator.SetBool("moving_up", facingDirection == Direction.UP);
            animator.SetBool("moving_down", facingDirection == Direction.DOWN);
            spriteRenderer.flipX = facingDirection == Direction.RIGHT;
        }

        public virtual void OnDeath(UnitContainer killedBy)
        {

        }

        public virtual void OnKill(UnitContainer killedUnit)
        {

        }

        public override ActionType GetAction()
        {
            return ActionType.All;
        }

        public override ActionType GetDefaultType()
        {
            return ActionType.Move;
        }
    }

}
