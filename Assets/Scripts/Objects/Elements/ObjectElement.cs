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

    /// <summary>
    /// Logoc class for all behaviour. Handles the logic for a specific object type.
    /// </summary>
    public abstract class ObjectElement : MonoBehaviour
    {
        /// <summary>
        /// Object that this element belongs to
        /// </summary>
        public abstract ObjectBehaviour Behaviour
        {
            get;
        }

    }

}
