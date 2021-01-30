using Steamwar.Factions;
using Steamwar.Grid;
using Steamwar.Objects;
using Steamwar.Supplies;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Steamwar.Core
{
    public class EventManager : Singleton<EventManager>
    {
        public ResourceEvent capacityUpdate;
        public ResourceEvent resourceUpdate;
    }

    [Serializable]
    public class ResourceEvent : UnityEvent<SupplyContainer>
    {

    }

}
