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
        public SessionEvent sessionLoaded;
        public SessionEvent sessionUpdated;
        public SessionEvent factionChanged;
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
        //IJudgeListener
        public ObjectEvent judgeChange;
        public ObjectEvent judgeClear;
    }

    [Serializable]
    public class ObjectEvent : UnityEvent<ObjectContainer>
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

    [Serializable]
    public class SessionEvent : UnityEvent<Session>
    {

    }

}
