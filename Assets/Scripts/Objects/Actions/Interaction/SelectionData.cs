using Steamwar.Buildings;
using Steamwar.Objects;
using Steamwar.Units;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Steamwar.Interaction
{
    /// <summary>
    /// Contains selection data for a object that was or is selected.
    /// </summary>
    public struct SelectionData
    {
        /// <summary>
        /// Default instance.
        /// <para/>
        /// This should be used if no object is currently selected.
        /// </summary>
        public static readonly SelectionData EMPTY = new SelectionData(null);
        private readonly ObjectBehaviour obj;
        private Vector3Int? _cellPos;


        public SelectionData(ObjectBehaviour obj)
        {
            this.obj = obj;
            this._cellPos = null;
        }

        /// <summary>
        /// True if this data contains no object and so no object is selected.
        /// </summary>
        public bool IsEmpty
        {
            get => Obj == null;
        }

        /// <summary>
        /// The cell position of the selected object.
        /// </summary>
        public Vector3Int CellPos
        {
            get
            {
                if (_cellPos == null)
                {
                    _cellPos = SessionManager.Instance.world.WorldToCell(obj.transform.position);
                }
                return _cellPos ?? Vector3Int.zero;
            }
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
            get => HasPlayerFaction && Unit != null && Unit.Data.CanMove;
        }


        /// <summary>
        /// //If this object has the faction of the player.
        /// </summary>
        public bool HasPlayerFaction
        {
            get => !IsEmpty && Obj.Data.faction.IsPlayer;
        }

        /// <summary>
        /// The current object.
        /// </summary>
        public ObjectBehaviour Obj => obj;


        /// <summary>
        /// Datermines if the given object is the same object as the selected object.
        /// </summary>
        /// <param name="other">An other object that should be compared to the selected object.</param>
        /// <returns>True if they are the same</returns>
        public bool IsSameObject(ObjectBehaviour other)
        {
            return other == Obj;
        }
    }
}
