using UnityEngine;
using System.Collections;
using Steamwar.Units;
using Steamwar.Utils;
using System;

namespace Steamwar.Units {
    public class AirshipElement : MonoBehaviour
    {

        public float scale;
        public float timeScale;
        [ReadOnly]
        public double time;
        [ReadOnly]
        public Vector3 origin;
        public Vector3 shadowOrigin;
        private UnitBehaviour unit;
        public GameObject shadow;

        void Start()
        {
            /*unit = GetComponentInParent<UnitBehaviour>();
            origin = unit.transform.position;
            if (shadow != null)
            {
                shadowOrigin = shadow.transform.localPosition;
            }*/
        }

        void Update()
        {
            /*time += Math.PI / 8;
            unit.transform.position = origin + Vector3.up * (float)Math.Sin(time * timeScale) * scale;
            if (shadow != null) {
                shadow.transform.localPosition = shadowOrigin + -Vector3.up * (float)Math.Sin(time * timeScale) * scale;
            }*/
        }
    }
}
