using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Steamwar.Objects;

namespace Steamwar.Buildings
{
    public class BuildingBehaviour : ObjectBehaviour<BuildingData, BuildingType, BuildingDataSerializable>
    {
        public override ObjectKind Kind
        {
            get=>ObjectKind.BUILDING;
        }
    }
}
