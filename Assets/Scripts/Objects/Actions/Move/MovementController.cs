using Steamwar;
using Steamwar.Move;
using Steamwar.Objects;
using Steamwar.Units;
using Steamwar.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steamwar.Move
{
    public class MovementController : SteamBehaviour
    {
        /// <summary>
        /// The object moved by this logic. Mostly the main object that contains the object.
        /// </summary>
        public ObjectContainer container;
        public GameObject controlledObject;
        public ObjectRenderer objectRenderer;
       /* public MovementState state;
        public IEnumerable<PathNode> path;
        public PathNode current;*/

        /// <summary>
        /// If the object currently can move.
        /// </summary>
        public virtual bool CanMove { get => IsMovable && !Moves; }

        /// <summary>
        /// If the object currently moves.
        /// </summary>
        public bool Moves
        {
            get; set;
        }

        /// <summary>
        /// If the object has the ability to move.
        /// </summary>
        public virtual bool IsMovable { get => true; }

        protected override void OnInit()
        {
            base.OnInit();
            if(objectRenderer == null)
            {
                objectRenderer = GetComponentInChildren<ObjectRenderer>();
            }
            if(container == null)
            {
                container = GetComponentInParent<ObjectContainer>();
            }
        }

        public void Move(IEnumerable<PathNode> path)
        {
            Moves = true;
            StartCoroutine(MoveUnit(path));
        }

        public IEnumerator MoveUnit(IEnumerable<PathNode> path)
        {
            Transform transform = controlledObject.transform;
            Direction facingDirection = Direction.LEFT;
            foreach (PathNode point in path)
            {
                Vector2 delta = ((Vector2)transform.position) - point.position;
                if (delta.x > 0)
                {
                    facingDirection = Direction.RIGHT;
                }
                else if (delta.y < 0)
                {
                    facingDirection = Direction.UP;
                }
                else if (delta.y > 0)
                {
                    facingDirection = Direction.DOWN;
                }
                else if (delta.x < 0)
                {
                    facingDirection = Direction.LEFT;
                }
                objectRenderer.UpdateAnimation(Moves, facingDirection);
                float sqrDistance = (((Vector2)transform.position) - point.position).sqrMagnitude;
                while (sqrDistance > float.Epsilon && ((Vector2)transform.position) != point.position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, point.position, 1F / container.Data.Type.Speed * Time.deltaTime);
                    sqrDistance = (((Vector2)transform.position) - point.position).sqrMagnitude;
                    yield return new WaitForEndOfFrame();
                }
            }
            Moves = false;
            objectRenderer.UpdateAnimation(Moves, facingDirection);
        }

        public enum MovementState
        {
            PRE,
            MOVING,
            END
        }
    }
}