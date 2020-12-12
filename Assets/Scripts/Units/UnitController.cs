using UnityEngine;
using System.Collections;
using System;

namespace Steamwar.Units
{
    public class UnitController : MonoBehaviour
    {
        public static UnitController instance = null;
        public UnitManager spawn = new UnitManager();

        public GameObject unitPrefab;
        public LayerMask unitLayer;
        public LayerMask groundLayer;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }

}