using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Steamwar.Objects {

    public abstract class ObjectElement : SteamBehaviour
    {
        protected ObjectContainer container;

        public ObjectContainer Container => container;

        protected override void OnSpawn()
        {
            if(container == null) {
                List<MonoBehaviour> results = new List<MonoBehaviour>();
                GetComponentsInParent(true, results);
                container = GetComponentInParent<ObjectContainer>();
            }
        }
    }

}
