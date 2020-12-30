
using Steamwar.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Buildings
{
    public static class BuildingExtensions
    {
        public delegate void BuildingAction(BuildingData data, BuildingType type, BuildingBehaviour building);

        public delegate bool BuildingPredicate(BuildingData data, BuildingType type, BuildingBehaviour building);

        public static void ActOnBuilding(this ObjectBehaviour source, BuildingPredicate predicate, BuildingAction action)
        {
            if (!(source is BuildingBehaviour))
            {
                return;
            }
            BuildingData data = (source as BuildingBehaviour).Data;
            if (data == null)
            {
                return;
            }
            BuildingType type = data.Type;
            if (type == null || !predicate(data, type, source as BuildingBehaviour))
            {
                return;
            }
            action(data, type, source as BuildingBehaviour);
        }

        public static void ActOnBuilding(this ObjectBehaviour source, BuildingAction action)
        {
            if (!(source is BuildingBehaviour))
            {
                return;
            }
            BuildingData data = (source as BuildingBehaviour).Data;
            if (data == null)
            {
                return;
            }
            BuildingType type = data.Type;
            if (type == null)
            {
                return;
            }
            action(data, type, source as BuildingBehaviour);
        }
    }

}
