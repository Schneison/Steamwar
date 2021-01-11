using Steamwar;
using Steamwar.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.UI
{
    public abstract class ExpandableElement :SteamBehaviour
    {
        [MyBox.ReadOnly]
        [SerializeField]
        private ExpandtionState state = ExpandtionState.Reduced;
        /// <summary>
        /// Number from 0 to 1. 0 => Not expanded / 1=> Fully expanded
        /// </summary>
        [MyBox.ReadOnly]
        [SerializeField]
        private float revealed = 0;

        /// <summary>
        /// Coroutine that is called at the spawn of this object
        /// </summary>
        private IEnumerator spawnCoroutine;

        public abstract GameObject Controlled
        {
            get;
        }

        public abstract Vector2 DefaultPos
        {
            get;
        }

        public abstract Vector2 ExpandedPos
        {
            get;
        }

        public abstract float Speed
        {
            get;
        }

        public bool IsExpanded => state == ExpandtionState.Expanded;
        public bool IsReduced => state == ExpandtionState.Reduced;
        public bool IsTransition => state == ExpandtionState.Reduces || state == ExpandtionState.Expands;

        public void SetExpanded(bool expanded)
        {
            bool transition = IsTransition;
            if (expanded)
            {
                if(state == ExpandtionState.Expanded)
                {
                    return;
                }
                state = ExpandtionState.Expands;
            }
            else
            {
                if (state == ExpandtionState.Reduced)
                {
                    return;
                }
                state = ExpandtionState.Reduces;
            }
            if (!transition && IsTransition)
            {
                if (!isActiveAndEnabled)
                {
                    spawnCoroutine = Expand();
                }
                else 
                {
                    StartCoroutine(Expand());
                }
            }
        }

        protected virtual void OnExpanded()
        {

        }

        protected virtual void OnReduced()
        {

        }

        protected void Update()
        {
            if(spawnCoroutine != null)
            {
                StartCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }

        private IEnumerator Expand()
        {
            RectTransform childTrans = Controlled.GetComponent<RectTransform>();
            while (state == ExpandtionState.Reduces && revealed > 0 || state == ExpandtionState.Expands && revealed < 1)
            {
                if (state == ExpandtionState.Reduces)
                {
                    revealed -= Time.deltaTime * Speed;
                }
                else
                {
                    revealed += Time.deltaTime * Speed;
                }
                revealed = MathHelper.Clamp(revealed, 0F, 1F);
                childTrans.anchoredPosition = Vector2.Lerp(DefaultPos, ExpandedPos, revealed);
                yield return null;
            }
            if(state == ExpandtionState.Reduces)
            {
                state = ExpandtionState.Reduced;
                OnReduced();
            }
            else
            {
                state = ExpandtionState.Expanded;
                OnExpanded();
            }
        }

        public void Clear()
        {
            RectTransform childTrans = Controlled.GetComponent<RectTransform>();
            childTrans.anchoredPosition = DefaultPos;
            state = ExpandtionState.Reduced;
            OnReduced();
            revealed = 0;
            spawnCoroutine = null;
        }
    }

    public enum ExpandtionState
    {
        None, 
        Expands,
        Reduces,
        Expanded,
        Reduced,
    }
}
