using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Steamwar.Resources
{

    public readonly struct ResourceList
    {
        public readonly int moneyAmount;

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

        public ResourceList With(Resource resource, int amount)
        {
            return new ResourceList(amount);
        }

        public int GetResource(Resource type)
        {
            return this[type];
        }

        public override bool Equals(object obj)
        {
            return obj is ResourceList list &&
                   moneyAmount == list.moneyAmount;
        }

        public override int GetHashCode()
        {
            return -925683117 + moneyAmount.GetHashCode();
        }

        public ResourceList Copy()
        {
            return new ResourceList(moneyAmount);
        }

        public static ResourceList operator +(ResourceList a) => a;

        public static ResourceList operator +(ResourceList a, ResourceList b) => new ResourceList(a.moneyAmount + b.moneyAmount);

        public static ResourceList operator -(ResourceList a, ResourceList b) => new ResourceList(a.moneyAmount - b.moneyAmount);

        public static ResourceList operator +(ResourceList a, ResourceProps b) => new ResourceList(a.moneyAmount + b.moneyAmount);

        public static ResourceList operator -(ResourceList a, ResourceProps b) => new ResourceList(a.moneyAmount - b.moneyAmount);

        public static ResourceList operator +(ResourceProps a, ResourceList b) => new ResourceList(a.moneyAmount + b.moneyAmount);

        public static ResourceList operator -(ResourceProps a, ResourceList b) => new ResourceList(a.moneyAmount - b.moneyAmount);

    }
}
