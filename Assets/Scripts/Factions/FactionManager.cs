
using Steamwar.Buildings;
using Steamwar.Core;
using Steamwar.Objects;
using Steamwar.Resources;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.PlayerLoop;

namespace Steamwar.Factions
{
    public class FactionManager : Singleton<FactionManager>
    {

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


        public static FactionData GetData(Faction faction)
        {
            return GetData(faction.index);
        }

        public static FactionData GetData(int index)
        {
            if (index >= SessionManager.session.factions.Length || index < 0)
            {
                return FactionData.None;
            }
            return SessionManager.session.factionDatas[index];
        }

        public static bool UpdatePrediction(int faction, PredictionHandler handler)
        {
            FactionData data = GetData(faction);
            if (!data.Exists)
            {
                return false;
            }
            data.Prediction = handler(data.Prediction);
            return true;
        }

        public static Faction GetFaction(ObjectBehaviour obj)
        {
            ObjectData data = obj.Data;
            if (data == null)
            {
                return Faction.None;
            }
            return data.faction;
           // return GetFaction(data.faction);
        }

        public static bool IsPlayerFaction(int index)
        {
            return SessionManager.session.playerIndex == index;
        }

        /// <summary>
        /// Called if an storage object gets added to the board.
        /// </summary>
        public void OnStorageAdded(ObjectBehaviour obj)
        {
            obj.ActOnBuilding(
                (data, type, building) => type.Tag.HasFlag(ObjectTag.Storage),
                (data, type, building) =>
                {
                    ResourceList capacity = type.storageCapacity;
                    UpdatePrediction(data.faction.index, (prediction) => prediction.WithCapacity(prediction.capacity + capacity));
                    EventManager.Instance.resourceUpdate.Invoke(data.faction.Data);
                });
        }

        /// <summary>
        /// Called if an storage object gets removed from the board.
        /// </summary>
        public void OnStorageRemoved(ObjectBehaviour obj)
        {
            obj.ActOnBuilding(
                (data, type, building) => type.Tag.HasFlag(ObjectTag.Storage),
                (data, type, building) =>
            {
                ResourceList capacity = type.storageCapacity;
                UpdatePrediction(data.faction.index, (prediction) => prediction.WithCapacity(prediction.capacity - capacity));
                EventManager.Instance.resourceUpdate.Invoke(data.faction.Data);
            });
        }

        /// <summary>
        /// Called after the board was created an every object constructed. 
        /// </summary>
        public void OnBoardCreated()
        {
            IDictionary<int, FactionPrediction> predictions = new Dictionary<int, FactionPrediction>();
            foreach(Faction faction in Factions)
            {
                predictions.Add(faction.index, FactionPrediction.Empty());
            }
            foreach(ObjectBehaviour obj in ObjectCache.GetObjects(ObjectTag.Storage))
            {
                obj.ActOnBuilding((data, type, building) =>
                {
                    FactionPrediction prediction = predictions[data.faction.index];
                    ResourceList capacity = type.storageCapacity;
                    predictions[data.faction.index] = prediction.WithCapacity(prediction.capacity + capacity);
                });
            }
            foreach (Faction faction in Factions)
            {
                UpdatePrediction(faction.index, (old)=>predictions[faction.index]);
            }
        }
    }
}
