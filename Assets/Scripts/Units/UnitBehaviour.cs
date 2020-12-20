using UnityEngine;
using Steamwar.Factions;
using System.Collections.Generic;
using System;
using Steamwar.Utils;
using Steamwar.Objects;
using Steamwar.Core;
using Steamwar.Move;

namespace Steamwar.Units {

    public class UnitBehaviour : DestroyableObject<UnitData, UnitType, UnitDataSerializable>
    {
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        internal bool needInit = false;
        internal Direction facingDirection;

        public override void Start()
        {
            base.Start();
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
                spriteRenderer.sprite = data.type.spriteBlue;
                
                needInit = false;
            }
        }

        protected override void Construction(UnitType type)
        {
            data = new UnitData
            {
                type = type,
                Health = type.Health,
                faction = SessionManager.session.playerFaction
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
            MovmentManager.Move(this, path);
        }

        /* Living */
        public virtual void Damage(UnitBehaviour attackedUnit)
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
            return data.Health;
        }

        public virtual float GetDamage(UnitBehaviour attackedUnit)
        {
            return data.type.damage;
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

        public virtual void OnDeath(UnitBehaviour killedBy)
        {

        }

        public virtual void OnKill(UnitBehaviour killedUnit)
        {

        }

        public override bool HasAction(ObjectBehaviour obj, ActionType type)
        {
            return true;
        }
    }

}
