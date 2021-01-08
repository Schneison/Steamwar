
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
        public delegate void BuildingAction(BuildingData data, BuildingType type, BuildingContainer building);

        public delegate bool BuildingPredicate(BuildingData data, BuildingType type, BuildingContainer building);

        public static void ActOnBuilding(this ObjectContainer source, BuildingPredicate predicate, BuildingAction action)
        {
            if (!(source is BuildingContainer))
            {
                return;
            }
            BuildingData data = (source as BuildingContainer).Data;
            if (data == null)
            {
                return;
            }
            BuildingType type = data.Type;
            if (type == null || !predicate(data, type, source as BuildingContainer))
            {
                return;
            }
            action(data, type, source as BuildingContainer);
        }

        public static void ActOnBuilding(this ObjectContainer source, BuildingAction action)
        {
            if (!(source is BuildingContainer))
            {
                return;
            }
            BuildingData data = (source as BuildingContainer).Data;
            if (data == null)
            {
                return;
            }
            BuildingType type = data.Type;
            if (type == null)
            {
                return;
            }
            action(data, type, source as BuildingContainer);
        }
    }

}
