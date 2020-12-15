using Steamwar.Buildings;
using Steamwar.Objects;
using Steamwar.Units;

namespace Steamwar.Interaction
{
    /// <summary>
    /// Contains selection data for a object that was or is selected.
    /// </summary>
    public struct SelectionData
    {
        public static readonly SelectionData EMPTY = new SelectionData(null);
        private readonly ObjectBehaviour obj;

        public SelectionData(ObjectBehaviour obj)
        {
            this.obj = obj;
        }

        public bool IsEmpty
        {
            get => Obj == null;
        }


        /// <summary>
        /// Retuns the object as a unit if the object is an unit.
        /// </summary>
        public UnitBehaviour Unit
        {
            get => Obj as UnitBehaviour;
        }

        /// <summary>
        /// Retuns the object as a building if the object is a building.
        /// </summary>
        public BuildingBehaviour Building
        {
            get => Obj as BuildingBehaviour;
        }

        /// <summary>
        /// If the player can move this unit.
        /// </summary>
        public bool AllowedToMove
        {
            get => HasPlayerFaction && Unit != null && Unit.data.CanMove;
        }


        /// <summary>
        /// //If this object has the faction of the player.
        /// </summary>
        public bool HasPlayerFaction
        {
            get => !IsEmpty && Obj.Data.faction.IsPlayer;
        }

        public ObjectBehaviour Obj => obj;

        public bool IsSameObject(ObjectBehaviour other)
        {
            return other == Obj;
        }
    }
}
