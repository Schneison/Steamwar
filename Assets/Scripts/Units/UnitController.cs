using UnityEngine;
using System.Collections;
using System;
using Steamwar.Utils;

namespace Steamwar.Units
{
    public class UnitController : Singleton<UnitController>
    {
        public UnitManager spawn = new UnitManager();

        public GameObject unitPrefab;
        public LayerMask unitLayer;
        public LayerMask groundLayer;

        void Start()
        {

        }

        void Update()
        {

        }
    }

}