using Steamwar.Objects;
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    public class Product
    {

    }

    public enum EffektType
    {
        UNIT,
    }

    [Serializable]
    public class EffektConfig {

        [Serializable]
        public class Object : EffektConfig
        {
            public ObjectType type;
        }

        [Serializable]
        public class Repair : EffektConfig
        {
            public int amount;
        }

    }

    [Serializable]
    public class FactoryConfig
    {
        public int progressTime;
        [Header("Effect")]
        public EffektType effectType;
        public EffektConfig effectConfig;
    }

    public class FactoryElement<O> : ObjectElement<O> where O : ObjectBehaviour
    {

        /// <summary>
        /// Current progress amount
        /// </summary>
        public int progress = 0;
        /// <summary>
        /// 
        /// </summary>
        public int progressMax = -1;

        public virtual int ProgressMax
        {
            get
            {
                if (progressMax < 0)
                {
                    progressMax = CalculateProgressrMax();
                }
                return progressMax;
            }
        }

        /// <summary>
        /// Can be used to calculate a max progress that the object should exceed to produce an effect.
        /// </summary>
        /// <returns>0 if the object produces nothing.</returns>
        public virtual int CalculateProgressrMax()
        {
            return 0;
        }

        /// <summary>
        /// Determines if the object can currently progress.
        /// </summary>
        /// <returns>True if the object can make progress</returns>
        public virtual bool CanProgress()
        {
            return false;
        }

        /// <summary>
        /// Determines if the object can start to produce
        /// </summary>
        /// <returns>True if the object can start the production</returns>
        public virtual bool CanStartProduction()
        {
            return false;
        }

        /// <summary>
        /// Called after the progress exceed the max progress
        /// </summary>
        public virtual void OnProduce()
        {

        }

        public void FixedUpdate()
        {
            if (ProgressMax > 0)
            {
                if (
                    progress == 0 && !CanStartProduction() ||
                    !CanProgress()
                    )
                {
                    return;
                }
                progress++;
                if (progress >= ProgressMax)
                {
                    progress = 0;
                    OnProduce();
                }
            }
        }
    }
}
