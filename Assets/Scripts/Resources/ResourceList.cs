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
        [Range(-1, 8192)]
        public int moneyAmount = -1;
    }
}
