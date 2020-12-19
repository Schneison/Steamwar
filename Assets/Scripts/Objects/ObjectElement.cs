using UnityEngine;
using System.Collections;

namespace Steamwar.Objects {

    public abstract class ObjectElement<O> : ObjectElement where O : ObjectBehaviour
    {
        protected O objectBehaviour;

        public override ObjectBehaviour Behaviour
        {
            get => objectBehaviour;
        }

        void Start()
        {
            objectBehaviour = GetComponentInParent<O>();
        }
    }

    public abstract class ObjectElement : MonoBehaviour
    {

        public abstract ObjectBehaviour Behaviour
        {
            get;
        }

    }

}
