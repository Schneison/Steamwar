
using Steamwar.Buildings;
using Steamwar.Core;
using Steamwar.Grid;
using Steamwar.Objects;
using Steamwar.Supplies;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Steamwar.Factions
{
    public class FactionManager : Singleton<FactionManager>
    {

        public delegate void DataAction(FactionData data);

        public FactionData factionPrefab;
        public FactionData[] datas;

        protected override void OnSpawn()
        {
            List<FactionData> factionData = new List<FactionData>();
            foreach(Faction faction in SessionManager.session.factions)
            {
                FactionData dataObj = Instantiate(factionPrefab, transform);
                dataObj.name = $"Faction({faction.index}={faction.name})";
                factionData.Add(dataObj);
                BoardManager.Instance.objectConstrcuted.AddListener(dataObj.Resources.OnObjectConstructed);
                BoardManager.Instance.objectDeconstructed.AddListener(dataObj.Resources.OnObjectDeconstructed);
            }
            datas = factionData.ToArray();
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
            return SessionManager.session.playerIndex == index;
        }
    }
}
