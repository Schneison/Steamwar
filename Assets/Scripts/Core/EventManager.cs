using Steamwar.Factions;
using Steamwar.Objects;
using Steamwar.Resources;
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
        // IBoardListener
        public UnityEvent boardCreation;
        // IBoardListener
        public ObjectEvent objectConstrcuted;
        // IBoardListener
        public ObjectEvent objectDeconstructed;
        // IFractionListener
        public FactionEvent factionUpdate;
        public ResourceEvent capacityUpdate;
        public ResourceEvent resourceUpdate;
    }

    [Serializable]
    public class ObjectEvent : UnityEvent<ObjectBehaviour>
    {
    }

    [Serializable]
    public class FactionEvent : UnityEvent<FactionData>
    {

    }

    [Serializable]
    public class ResourceEvent : UnityEvent<ResourceContainer>
    {

    }

}
