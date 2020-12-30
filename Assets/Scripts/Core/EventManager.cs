using Steamwar.Factions;
using Steamwar.Objects;
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
        public UnityEvent boardCreation;
        public ObjectEvent objectConstrcuted;
        public ObjectEvent objectDeconstrcuted;
        public FactionEvent predictionUpdate;
        public FactionEvent resourceUpdate;
    }

    [Serializable]
    public class ObjectEvent : UnityEvent<ObjectBehaviour>
    {
    }

    [Serializable]
    public class FactionEvent : UnityEvent<FactionData>
    {

    }

}
