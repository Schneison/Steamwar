using System.Collections;
using UnityEngine;
using Steamwar.Utils;

namespace Steamwar.Supplies
{
    public class SupplyManager : Singleton<SupplyManager>
    {

        public delegate void ResourceUpdateEvent(int factionId, Supply resource, int amount, int maxAmount);


        void Update()
        {

        }
    }
}