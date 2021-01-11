using UnityEngine;
using System.Collections;
using Steamwar.Units;
using Steamwar.Utils;
using System;
using MyBox;

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
        private SpriteRenderer unitRenderer;

        void Start()
        {
            unitRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
            origin = unitRenderer.transform.localPosition;
        }

        void Update()
        {
            time += Math.PI / 8;
            unitRenderer.transform.localPosition = origin + Vector3.up * (float)Math.Sin(time * timeScale) * scale;
        }
    }
}
