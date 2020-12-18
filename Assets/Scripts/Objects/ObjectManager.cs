using UnityEngine;
using System.Collections;
using Steamwar.Utils;

namespace Steamwar.Objects
{
    public class ObjectManager : Singleton<ObjectManager>
    {
        [Header("Layers")]
        public LayerMask unitLayer;
        public LayerMask groundLayer;
        public void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
