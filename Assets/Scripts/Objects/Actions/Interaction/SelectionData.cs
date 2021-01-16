using Steamwar.Buildings;
using Steamwar.Factions;
using Steamwar.Grid;
using Steamwar.Move;
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
        private readonly ObjectContainer obj;
        private readonly SelectionBehaviour objSelectable;
        private readonly MovementController movement;
        private Vector3Int? _cellPos;


        public SelectionData(ObjectContainer obj)
        {
            this.obj = obj;
            this._cellPos = null;
            this.objSelectable = obj != null ? obj.GetComponent<SelectionBehaviour>() : null;
            this.movement = obj != null ? obj.GetComponent<MovementController>() : null;
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
                    _cellPos = BoardManager.WorldToCell(obj.transform.position);
                }
                return _cellPos ?? Vector3Int.zero;
            }
        }

        /// <summary>
        /// If the player can move this unit.
        /// </summary>
        public bool AllowedToMove
        {
            get => HasPlayerFaction && Movement != null && Movement.CanMove;
        }


        /// <summary>
        /// //If this object has the faction of the player.
        /// </summary>
        public bool HasPlayerFaction
        {
            get => !IsEmpty && Obj.Data.HasPlayerFaction();
        }

        /// <summary>
        /// The current object.
        /// </summary>
        public ObjectContainer Obj => obj;

        public SelectionBehaviour Selectable => objSelectable;

        public MovementController Movement => movement;


        /// <summary>
        /// Datermines if the given object is the same object as the selected object.
        /// </summary>
        /// <param name="other">An other object that should be compared to the selected object.</param>
        /// <returns>True if they are the same</returns>
        public bool IsSameObject(ObjectContainer other)
        {
            return other == Obj;
        }
    }
}
