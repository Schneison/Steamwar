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

        // Update is called once per frame
        void Update()
        {

        }
    }
}
