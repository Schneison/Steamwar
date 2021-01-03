using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Steamwar.Utils;

namespace Steamwar
{
    public class SteamBehaviour : MonoBehaviour
    {
        private bool _initialized = false;
        private bool _spawned = false;

        /*private EventSystem eventSystem;
        public EventSystem GetEventSystem()
        {
            if (eventSystem == null)
            {
                eventSystem = new EventSystem();
            }
            return eventSystem;
        }

        public bool HasEventSystem => eventSystem != null;*/

        protected virtual void Awake()
        {
            if (GameManager.ShuttDown())
            {
                return;
            }
            InitializedComponent();
        }

        protected void InitializedComponent()
        {
            if (_initialized)
                return;
            OnInit();
            _initialized = true;
        }

        public void Start()
        {
            if (GameManager.ShuttDown())
            {
                return;
            }
            this.Spawn();
        }

        public void Spawn()
        {
            if (_spawned)
                return;
            if (!_initialized)
            {
                Debug.LogError(name + "." + GetType().Name + " is not initialized.");
            }
            else
            {
                _spawned = true;
                try
                {
                    OnSpawn();
                }
                catch (Exception ex)
                {
                    DebugUtil.LogException(this, "Error in " + name + "." + GetType().Name + ".OnSpawn", ex);
                }
            }
        }

        private void OnEnable()
        {
            if (GameManager.ShuttDown())
            {
                return;
            }
            OnCompEnable();
        }

        private void OnDisable()
        {
            if (GameManager.ShuttDown())
            {
                return;
            }
            OnCompDisable();
        }

        private void OnDestroy()
        {
            if (GameManager.ShuttDown())
            {
                return;
            }
            OnCleanUp();
        }

        protected virtual void OnInit()
        {

        }

        protected virtual void OnSpawn()
        {
        }

        protected virtual void OnCompEnable()
        {
        }

        protected virtual void OnCompDisable()
        {
        }

        protected virtual void OnCleanUp()
        {

        }
    }
}
