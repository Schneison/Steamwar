
using Steamwar.Buildings;
using Steamwar.Core;
using Steamwar.Grid;
using Steamwar.Objects;
using Steamwar.Supplies;
using Steamwar.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace Steamwar.Factions
{
    public class FactionManager : Singleton<FactionManager>, ISessionListener, IService
    {

        public FactionDataEvent dataUpdate;
        public FactionStateEvent factionActivated;
        public FactionStateEvent factionDeactivated;

        public delegate void DataAction(FactionData data);

        public FactionData factionPrefab;
        public FactionData[] datas;
        public int activeFaction;
        public int playerFaction;
        public Faction[] factions;

        protected override void OnInit()
        {
            Services.factions.Create<FactionManager>((state) => state == LifecycleState.SESSION, ()=> new ServiceContainer[] { Services.session });
        }

        public void OnCreateSession(Session session)
        {
            session.factionOrder = (from faction in session.factions orderby faction.index select faction.index).ToArray();
            session.activeFaction = -1;
        }

        public void OnLoadSession(Session session)
        {
            this.factions = session.factions;
            this.playerFaction = session.playerIndex;
            this.activeFaction = session.activeFaction;

            List<FactionData> factionData = new List<FactionData>();
            foreach (Faction faction in factions)
            {
                FactionData dataObj = Instantiate(factionPrefab, transform);
                dataObj.name = $"Faction({faction.index}={faction.name})";
                factionData.Add(dataObj);
                BoardManager.Instance.objectConstrcuted.AddListener(dataObj.Resources.OnObjectConstructed);
                BoardManager.Instance.objectDeconstructed.AddListener(dataObj.Resources.OnObjectDeconstructed);
            }
            datas = factionData.ToArray();
        }

        public void OnSaveSession(Session session)
        {
            session.factions = this.factions;
            session.activeFaction = activeFaction;
        }

        public void OnFinishLoading()
        {
        }

        public static Faction GetFaction(int index)
        {
            if(index >= SessionManager.session.factions.Length || index < 0)
            {
                return Faction.None;
            }
            return SessionManager.session.factions[index];
        }

        public static Faction[] Factions
        {
            get => SessionManager.session.factions;
        }

        public static FactionData GetData(ObjectContainer obj)
        {
            Faction faction = GetFaction(obj);
            if(faction == Faction.None)
            {
                return null;
            }
            return GetData(faction.index);
        }

        public static FactionData GetData(Faction faction)
        {
            return GetData(faction.index);
        }

        public static FactionData GetData(int index)
        {
            if (index >= Instance.datas.Length || index < 0)
            {
                return null;
            }
            return Instance.datas[index];
        }

        public static bool UpdateData(int faction, DataAction handler)
        {
            FactionData data = GetData(faction);
            if (!data.Exists())
            {
                return false;
            }
            handler(data);
            return true;
        }

        public static Faction GetFaction(ObjectContainer obj)
        {
            ObjectData data = obj.Data;
            if (data == null)
            {
                return Faction.None;
            }
            return GetFaction(data.FactionIndex);
           // return GetFaction(data.faction);
        }

        public static bool IsPlayerFaction(int index)
        {
            return Instance.playerFaction == index;
        }

        public static void Activate(int index)
        {
            Instance.ActivateFaction(index);
        }

        private void ActivateFaction(int index)
        {
            Faction oldFaction = GetFaction(activeFaction);
            if (oldFaction.Exists) {
                factionDeactivated.Invoke(GetFaction(activeFaction));
            }
            activeFaction = index;
            Faction newFaction = GetFaction(activeFaction);
            if (newFaction.Exists)
            {
                factionActivated.Invoke(GetFaction(activeFaction));
            }
        }

        public IEnumerator Initialize()
        {
            yield return null;
        }

        public IEnumerator CleanUp()
        {
            yield return null;
        }
    }

    [Serializable]
    public class FactionDataEvent : UnityEvent<FactionData>
    {

    }

    [Serializable]
    public class FactionStateEvent : UnityEvent<Faction>
    {

    }
}
