using MyBox;
using Steamwar.Core;
using Steamwar.Objects;
using Steamwar.Buildings;
using Steamwar.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamwar.Utils;
using System.Collections;
using UnityEngine;
using Steamwar.Factions;

namespace Steamwar.Resources
{
    [Serializable]
    public class ResourceContainer : SteamBehaviour, IBoardListener
    {
        private readonly CapacityCallback _capacity;
        private readonly IDictionary<string, int> _capacities = new Dictionary<string, int>();
        private bool _callEvents = true;
        ///Serialised
        public MyDictionary<string, int> resources = new MyDictionary<string, int>();
        private int faction;

        public ResourceContainer()
        {
            _capacity = new CapacityCallback(this);
        }

        public bool IsPlayer => FactionManager.IsPlayerFaction(faction);

        protected override void OnSpawn()
        {
            faction = GetComponent<FactionData>().factionIndex;
            foreach (Resource resource in SessionManager.registry.GetResources())
            {
                if (!resources.ContainsKey(resource.id))
                {
                    resources[resource.id] = 0;
                }
                _capacities[resource.id] = 0;
            }
        }

        public int this[Resource resource]
        {
            get => resources[resource.id];

            set
            {
                int current = resources[resource.id];
                int newValue = MathHelper.Clamp(value, 0, Capacity[resource.id]);
                if (newValue != current)
                {
                    resources[resource.id] = newValue;
                    if (_callEvents)
                    {
                        EventManager.Instance.resourceUpdate.Invoke(this);
                    }
                }
            }
        }

        public CapacityCallback Capacity
        {
            get => _capacity;
        }

        public int GetCapacity(Resource resource)
        {
            return _capacities[resource.id];
        }

        public void OnObjectConstructed(ObjectBehaviour obj)
        {
            obj.ActOnBuilding(
               (data, type, building) => data.HasFaction(faction) && type.Tag.HasFlag(ObjectTag.Storage),
               (data, type, building) =>
               {
                   ResourceProps capacity = type.storageCapacity;
                   if(capacity == null)
                   {
                       return;
                   }
                   _callEvents = false;
                   foreach (string resource in resources.Keys)
                   { 
                       Capacity[resource] += capacity[resource];
                   }
                   EventManager.Instance.capacityUpdate.Invoke(this);
                   _callEvents = true;
               });
        }

        public void OnObjectDeconstructed(ObjectBehaviour obj)
        {
            obj.ActOnBuilding(
                 (data, type, building) => data.HasFaction(faction) && type.Tag.HasFlag(ObjectTag.Storage),
                 (data, type, building) =>
                 {
                     ResourceProps capacity = type.storageCapacity;
                     if (capacity == null)
                     {
                         return;
                     }
                     _callEvents = false;
                     foreach (string resource in resources.Keys)
                     {
                         Capacity[resource] -= capacity[resource];
                     }
                     EventManager.Instance.capacityUpdate.Invoke(this);
                     _callEvents = true;
                 });
        }


        /// <summary>
        /// Called after the board was created an every object constructed. 
        /// </summary>
        /*public void OnBoardCreated()
        {
            IDictionary<int, FactionPrediction> predictions = new Dictionary<int, FactionPrediction>();
            foreach (Faction faction in Factions)
            {
                predictions.Add(faction.index, FactionPrediction.Empty());
            }
            foreach (ObjectBehaviour obj in Objects.Board.GetObjects(ObjectTag.Storage))
            {
                obj.ActOnBuilding((data, type, building) =>
                {
                    FactionPrediction prediction = predictions[data.faction.index];
                    ResourceProps capacity = type.storageCapacity;
                    predictions[data.faction.index] = prediction.AddCapacity(capacity);
                });
            }
            foreach (Faction faction in Factions)
            {
                UpdatePrediction(faction.index, (old) => predictions[faction.index]);
            }
        }*/

        public class CapacityCallback
        {
            private readonly ResourceContainer container;

            public CapacityCallback(ResourceContainer container)
            {
                this.container = container;
            }

            public int this[Resource resource]
            {
                get => this[resource.id];
                set => this[resource.id] = value;
            }

            public int this[string resourceName]
            {
                get => container._capacities[resourceName];
                set
                {
                    if (container._capacities[resourceName] != value)
                    {
                        container._capacities[resourceName] = value;
                        if (container._callEvents)
                        {
                            EventManager.Instance.capacityUpdate.Invoke(container);
                        }
                    }
                }
            }
        }

    }
}
