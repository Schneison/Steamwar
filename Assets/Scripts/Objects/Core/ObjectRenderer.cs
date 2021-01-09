using Steamwar.Factions;
using Steamwar.Units;
using Steamwar.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Steamwar.Objects
{
    public class ObjectRenderer : SteamBehaviour
    {
        public ObjectContainer obj;
        SpriteRenderer[] baseRenderer;
        SpriteRenderer[] colorRenderer;
        SpriteRenderer[] renderers;
        Animator[] baseAnimators;
        Animator[] colorAnimators;
        Animator[] animators;
        Direction _facingDirection;
        bool _moves;

        protected override void OnInit()
        {
            base.OnInit();
            if (obj == null) {
                obj = GetComponentInParent<ObjectContainer>();
            }
            Transform baseTransorm = transform.Find("Base");
            Transform colorTransorm = transform.Find("Color");
            baseRenderer = baseTransorm.GetComponentsInChildren<SpriteRenderer>();
            colorRenderer = colorTransorm.GetComponentsInChildren<SpriteRenderer>();
            renderers = baseRenderer.Merge(colorRenderer);
            baseAnimators = baseTransorm.GetComponentsInChildren<Animator>();
            colorAnimators = colorTransorm.GetComponentsInChildren<Animator>();
            animators = baseAnimators.Merge(colorAnimators);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            ObjectData data = obj.Data;
            Faction faction = data.GetFaction();
            RuntimeAnimatorController[] animations = data.Type.animations;
            RuntimeAnimatorController[] coloredCnimations = data.Type.coloredAnimations;
            int i;
            for (i = 0; i < baseRenderer.Length; i++)
            {
                if (i >= data.Type.baseSprites.Length)
                {
                    break;
                }
                SpriteRenderer render = baseRenderer[i];
                render.sprite = data.Type.baseSprites[i];
                if(baseAnimators.Length > i) {
                baseAnimators[i].runtimeAnimatorController = animations.Length > i ? animations[i] : null;
                }
            }
            for (i= 0;i< colorRenderer.Length;i++)
            {
                if(i >= data.Type.coloredSprites.Length)
                {
                    break;
                }
                SpriteRenderer render = colorRenderer[i];
                render.sprite = data.Type.coloredSprites[i];
                render.color = faction.color;
                if (colorAnimators.Length > i)
                {
                    colorAnimators[i].runtimeAnimatorController = coloredCnimations.Length > i ? coloredCnimations[i] : null;
                }
            }
        }

        public void UpdateAnimation(bool moves, Direction facingDirection)
        {
            if(_facingDirection == facingDirection && _moves == moves)
            {
                return;
            }
            _facingDirection= facingDirection;
            _moves = moves;
            foreach (Animator animator in animators)
            {
                if(!animator.isInitialized || animator.runtimeAnimatorController == null)
                {
                    continue;
                }
                animator.SetBool("moving", moves && (facingDirection == Direction.LEFT || facingDirection == Direction.RIGHT));
                animator.SetBool("moving_up", facingDirection == Direction.UP);
                animator.SetBool("moving_down", facingDirection == Direction.DOWN);
            }
            foreach(SpriteRenderer renderer in renderers)
            {
                renderer.flipX = facingDirection == Direction.RIGHT;
            }
        }

        public static GameObject CreateFor(ObjectType type, Faction faction)
        {
            GameObject renderObj = new GameObject("Renderer", typeof(ObjectRenderer));
            GameObject baseObj = new GameObject("Base");
            baseObj.transform.parent = renderObj.transform;
            GameObject colorObj = new GameObject("Color");
            colorObj.transform.parent = renderObj.transform;
            void applySprite(GameObject obj, int index, bool colored)
            {
                SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                renderer.sprite = type.baseSprites[index];
                renderer.sortingLayerName = "Objects";
                renderer.sortingOrder = index;
                if (colored)
                {
                    renderer.color = faction.color;
                }
            }
            int i;
            for (i = 0; i < type.baseSprites.Length; i++)
            {
                GameObject baseSprite = new GameObject("0", typeof(SpriteRenderer));
                baseSprite.transform.parent = baseObj.transform;
                applySprite(baseSprite, i, false);
                GameObject colorSprite = new GameObject("0", typeof(SpriteRenderer));
                colorSprite.transform.parent = colorObj.transform;
                applySprite(colorSprite, i, true);
            }
            return renderObj;
         }
    }
}