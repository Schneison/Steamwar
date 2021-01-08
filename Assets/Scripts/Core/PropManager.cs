using UnityEngine;
using System.Collections.Generic;
using Steamwar.Objects;
using Steamwar.Utils;

namespace Steamwar.Core
{
    public class PropManager : Singleton<PropManager>
    {
        public Stack<ObjectContainer> props = new Stack<ObjectContainer>();
        private bool _init = false;

        public static void CheckForProp(ObjectContainer behaviour)
        {
            if(!Instance._init)
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
                if (!_init)
                {
                    _init = true;
                }
            }
        }
    }
}
