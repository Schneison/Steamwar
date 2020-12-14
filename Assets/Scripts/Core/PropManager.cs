using UnityEngine;
using System.Collections.Generic;
using Steamwar.Objects;

namespace Steamwar.Core
{
    public class PropManager : MonoBehaviour
    {
        public Stack<ObjectBehaviour> props = new Stack<ObjectBehaviour>();
        private bool _initialized = false;
        public static PropManager instance;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        public static void CheckForProp(ObjectBehaviour behaviour)
        {
            if(!instance._initialized)
            {
                instance?.props?.Push(behaviour);
            }
        }

        void Update()
        {
            while (props.Count > 0)
            {
                var prop = props.Pop();
                prop.OnPropInit();

            }
            if (!_initialized)
            {
                _initialized = true;
            }
        }
    }
}
