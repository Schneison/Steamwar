using UnityEngine;
using System.Collections;

namespace Steamwar.Objects {

    public abstract class ObjectElement<O> : ObjectElement where O : ObjectContainer
    {
        protected O objectBehaviour;

        public override ObjectContainer Behaviour
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
        public abstract ObjectContainer Behaviour
        {
            get;
        }

    }

}
