using Steamwar.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Factions
{
    [Serializable]
    public struct FactionData
    {
        public readonly int factionIndex;
        public ResourceList resources;
        public FactionPrediction prediction;

        public FactionData(int factionIndex)
        {
            this.factionIndex = factionIndex;
            this.resources = new ResourceList();
            this.prediction = CreatePrediction();
        }

        public void Update()
        {
            UpdatePrediction();
        }

        public static FactionPrediction CreatePrediction()
        {
            return new FactionPrediction();
        }

        public void UpdatePrediction()
        {

        }
    }
}
