using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Steamwar.Objects {

    public abstract class DestroyableObject<D, T, S> : ObjectBehaviour<D, T, S> where D : DestroyableData<T, S>, new() where T : ObjectType where S : DestroyableDataSerializable, new()
    {
        private HealthIndicator healthIndicator;

        public override void Start()
        {
            base.Start();
            healthIndicator = GetComponentInChildren<HealthIndicator>(true);
        }
        public override void OnDeselection()
        {
            if (healthIndicator != null)
            {
                healthIndicator.gameObject.SetActive(false);
            }
        }

        public override void OnSelection()
        {
            if (healthIndicator != null)
            {
                healthIndicator.gameObject.SetActive(true);
            }
        }

    }
}
