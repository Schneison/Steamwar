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

        /// <summary>
        /// The config of this factory.
        /// </summary>
        public FactoryConfig config;

        /// <summary>
        /// The config for the currently queued product
        /// </summary>
        [SerializeField]
        private ProductConfig _product;
        /// <summary>
        /// The data for the currently qued product
        /// </summary>
        public ProductContext context;

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

        public ProductConfig Product {
            get
            {
                if((_product == null || _product.name.Length == 0) && (config != null && config.automatic))
                {
                    _product = config.GetAutomaticProduct();
                    context = new ProductContext(config.automaticName, Vector3Int.up);
                }
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        public void SetProduct(ProductConfig product)
        {

        }

        /// <summary>
        /// Can be used to calculate a max progress that the object should exceed to produce an effect.
        /// </summary>
        /// <returns>0 if the object produces nothing.</returns>
        public virtual int CalculateProgressrMax()
        {
            return Product?.progressTime ?? 0;
        }

        /// <summary>
        /// Determines if the object can currently progress.
        /// </summary>
        /// <returns>True if the object can make progress</returns>
        public virtual bool CanProgress()
        {
            return Product?.Config?.CanProgress(Behaviour, context) ?? false;
        }

        /// <summary>
        /// Determines if the object can start to produce
        /// </summary>
        /// <returns>True if the object can start the production</returns>
        public virtual bool CanStartProduction()
        {
            return Product?.Config?.CanStartProduction(Behaviour, context) ?? false;
        }

        /// <summary>
        /// Called after the progress exceed the max progress
        /// </summary>
        public virtual void OnProduce()
        {
            Product?.Config?.OnProduce(Behaviour, context);
        }

        public void FixedUpdate()
        {
            if (Product != null && ProgressMax > 0)
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
