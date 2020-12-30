using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Steamwar.Resources
{
    [Serializable]
    public class ResourceList
    {
        [Range(0, 8192)]
        public int moneyAmount = 0;

        public ResourceList()
        {
            this.moneyAmount = 0;
        }

        public ResourceList(int moneyAmount)
        {
            this.moneyAmount = moneyAmount;
        }

        public int this[Resource type]
        {
            get
            {
                return moneyAmount;
            }
        }

        public static ResourceList operator +(ResourceList a) => a;

        public static ResourceList operator +(ResourceList a, ResourceList b) => new ResourceList(a.moneyAmount + b.moneyAmount);

        public static ResourceList operator -(ResourceList a, ResourceList b) => new ResourceList(a.moneyAmount - b.moneyAmount);
    }
}
