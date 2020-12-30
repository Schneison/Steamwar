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
    public class FactionData
    {
        public static readonly FactionData None = new FactionData(-1);

        public readonly int factionIndex;
        private ResourceList resources;
        private FactionPrediction prediction;

        public FactionData(int factionIndex)
        {
            this.factionIndex = factionIndex;
            this.resources = new ResourceList();
            this.prediction = CreatePrediction();
        }

        public bool IsPlayer
        {
            get => FactionManager.IsPlayerFaction(factionIndex);
        }

        public bool Exists
        {
            get => factionIndex >= 0;
        }
        public FactionPrediction Prediction
        {
            get => prediction;
            set => prediction = value;
        }
        public ResourceList Resources 
        { 
            get => resources; 
            set => resources = value; 
        }

        public void Update()
        {
            UpdatePrediction();
        }

        public static FactionPrediction CreatePrediction()
        {
            return FactionPrediction.Empty();
        }

        public void UpdatePrediction()
        {

        }
    }
}
