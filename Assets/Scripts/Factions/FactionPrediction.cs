using Steamwar.Objects;
using Steamwar.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Factions
{
    public delegate FactionPrediction PredictionHandler(FactionPrediction current);

    public struct FactionPrediction
    {
        public readonly ResourceList capacity;

        public FactionPrediction(ResourceList capacity)
        {
            this.capacity = capacity;
        }

        public FactionPrediction WithCapacity(ResourceList capacity)
        {
            return new FactionPrediction(capacity);
        }

        public static FactionPrediction Empty()
        {
            return new FactionPrediction(new ResourceList());
        }

        public static void onUpdate(int factionIndex)
        {

        }

    }
}
