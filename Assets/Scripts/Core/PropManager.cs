using UnityEngine;
using System.Collections.Generic;
using Steamwar.Objects;
using Steamwar.Utils;

namespace Steamwar.Core
{
    public class PropManager : Singleton<PropManager>
    {
        public Stack<ObjectBehaviour> props = new Stack<ObjectBehaviour>();
        private bool _initialized = false;

        public static void CheckForProp(ObjectBehaviour behaviour)
        {
            if(!Instance._initialized)
            {
                Instance.props?.Push(behaviour);
            }
        }

        void Update()
        {
            if(SessionManager.session != null) {
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
}
