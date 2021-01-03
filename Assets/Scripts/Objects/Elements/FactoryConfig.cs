using MyBox;
using Steamwar.Factions;
using Steamwar.Resources;
using Steamwar.Utils;
using System;
using System.Linq;
using System.Security.AccessControl;
using UnityEditor;
using UnityEngine;

namespace Steamwar.Objects
{
    /// <summary>
    /// All possible effect types.
    /// </summary>
    public enum EffektType
    {
        None,
        Object,
        Repair,
        Resource
    }

    /// <summary>
    /// Defines a possible effect of a product.
    /// </summary>
    [Serializable]
    public class EffektConfig
    {

        /// <summary>
        /// Can be used to calculate a max progress that the object should exceed to produce an effect.
        /// </summary>
        /// <returns>0 if the object produces nothing.</returns>
        public virtual int CalculateProgressrMax(ObjectBehaviour obj)
        {
            return 0;
        }

        /// <summary>
        /// Determines if the object can currently progress.
        /// </summary>
        /// <returns>True if the object can make progress</returns>
        public virtual bool CanProgress(ObjectBehaviour obj, ProductContext context)
        {
            return false;
        }

        /// <summary>
        /// Determines if the object can start to produce
        /// </summary>
        /// <returns>True if the object can start the production</returns>
        public virtual bool CanStartProduction(ObjectBehaviour obj, ProductContext context)
        {
            return false;
        }

        /// <summary>
        /// Called after the progress exceed the max progress
        /// </summary>
        public virtual void OnProduce(ObjectBehaviour obj, ProductContext context)
        {

        }

        /// <summary>
        /// Defines an effect that creates an object.
        /// </summary>
        [Serializable]
        public class Object : EffektConfig
        {
            public ResourceProps resources;
            public ObjectType type;
        }

        /// <summary>
        /// Defines an effect that repairs an object.
        /// </summary>
        [Serializable]
        public class Repair : EffektConfig
        {
            public int amount;

        }

        /// <summary>
        /// Defines an effect that produces a resource.
        /// </summary>
        [Serializable]
        public class Resource : EffektConfig
        {
            public Resources.Resource type;
            public int amount;

            public override bool CanProgress(ObjectBehaviour obj, ProductContext context)
            {
                return true;
            }

            public override bool CanStartProduction(ObjectBehaviour obj, ProductContext context)
            { 
                FactionData data = FactionManager.GetData(obj);
                int amount = data.Exists() ? data.Resources[type] : 0;
                int maxAmount = data.Exists() ? data.Resources.Capacity[type] : 0;
                return amount < maxAmount;
            }

            public override void OnProduce(ObjectBehaviour obj, ProductContext context)
            {
                FactionData data = obj.Data.faction.Data;
                if(data.Exists())
                {
                    data.Resources[type] += amount;
                }
            }
        }

    }

    /// <summary>
    /// Defines a possible product for a factory.
    /// </summary>
    [Serializable]
    public class ProductConfig
    {
        public static readonly ProductConfig Missing = new ProductConfig("missing");
        /// <summary>
        /// The time needed to produce this product.
        /// </summary>
        public int progressTime;
        /// <summary>
        /// TThe name of this product
        /// </summary>
        public string name;
        /// <summary>
        /// The selected effect
        /// </summary>
        public EffektType effectType;
        [ConditionalField(nameof(effectType), false, nameof(EffektType.Object))]
        public EffektConfig.Object objectConfig;
        [ConditionalField(nameof(effectType), false, nameof(EffektType.Repair))]
        public EffektConfig.Repair repairConfig;
        [ConditionalField(nameof(effectType), false, nameof(EffektType.Resource))]
        public EffektConfig.Resource resourceConfig;

        public ProductConfig()
        {
        }

        private ProductConfig(string name)
        {
            this.name = name;
            this.progressTime = int.MaxValue;
            this.effectType = EffektType.None;
        }

        /// <summary>
        /// Returns the config for the effect of this product.
        /// </summary>
        public EffektConfig Config
        {
            get
            {
                return effectType switch
                {
                    EffektType.Object => objectConfig,
                    EffektType.Repair => repairConfig,
                    EffektType.Resource => resourceConfig,
                    _ => default,
                };
            }
        }
    }

    /// <summary>
    /// Context for a product that is currently queued
    /// </summary>
    [Serializable]
    public class ProductContext
    {
        public string productName;
        public Vector3Int cellPos;

        public ProductContext(string productName, Vector3Int cellPos)
        {
            this.productName = productName;
            this.cellPos = cellPos;
        }
    }


    /// <summary>
    /// Configuration type for a factory
    /// </summary>
    [Serializable]
    public class FactoryConfig : ScriptableObject
    {
        /// <summary>
        /// A list of all products of this factory.
        /// </summary>
        public ProductConfig[] products;
        /// <summary>
        /// if the factory should try to produce a product automatically if no other product is queued
        /// </summary>
        public bool automatic;
        /// <summary>
        /// The product the factory should create automatically.
        /// </summary>
        public string automaticName;

        public ProductConfig GetAutomaticProduct()
        {
            return GetProduct(automaticName);
        }

        public ProductConfig GetProduct(string name)
        {
            return products
                .Where((product)=>product.name == name)
                .FirstOrDefault() 
                ?? ProductConfig.Missing;
        }

        [MenuItem("Create/Factory Config")]
        public static void CreateType()
        {
            ScriptableObjectUtility.CreateAsset<FactoryConfig>();
        }
    }
}