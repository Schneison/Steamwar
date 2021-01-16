using UnityEngine;
using System.Collections.Generic;
using Steamwar.Objects;
using Steamwar.Utils;
using System.Collections;

namespace Steamwar.Core
{
    public class PropManager : Singleton<PropManager>, IService
    {
        public Stack<ObjectContainer> props = new Stack<ObjectContainer>();
        private bool _init = false;

        protected override void OnInit()
        {
            Services.props.Create<PropManager>((state)=>state == LifecycleState.SESSION, ()=>new ServiceContainer[] { Services.board });
        }

        public static void CheckForProp(ObjectContainer behaviour)
        {
            if(!Instance._init)
            {
                Instance.props?.Push(behaviour);
            }
        }

        public IEnumerator CleanUp()
        {
            yield return null;
        }

        public IEnumerator Initialize()
        {
            while (props.Count > 0)
            {
                var prop = props.Pop();
                prop.OnPropInit();

            }
            if (!_init)
            {
                _init = true;
            }
            yield return null;
        }
    }
}
