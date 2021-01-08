using Steamwar;
using Steamwar.Objects;
using System.Collections;
using UnityEngine;

namespace Steamwar.Interaction
{

    public class SelectionBehaviour : SteamBehaviour
    {

        /// <summary>
        /// If the object is currently selected by the player.
        /// </summary>
        public bool Selected { get; private set; }

        /// <summary>
        /// Selects the object for the player
        /// </summary>
        /// <returns>True if the object was selected.</returns>
        public bool Select()
        {
            if (!IsSelectable() || Selected)
            {
                return false;
            }
            OnSelection();
            Selected = true;
            return true;
        }

        /// <summary>
        /// Called at the moment the object gets selected
        /// </summary>
        public virtual void OnSelection()
        {
            // Empty by default
        }

        /// <summary>
        /// If the object can be selected by the player.
        /// </summary>
        /// <returns>True if the object can be selected.</returns>
        protected virtual bool IsSelectable()
        {
            return true;
        }

        /// <summary>
        /// Deselects the object for the player
        /// </summary>
        /// <returns>True if the object was deselected.</returns>
        public bool Deselect()
        {
            if (!IsDeselectable() || !Selected)
            {
                return false;
            }
            Selected = false;
            OnDeselection();
            return true;
        }

        /// <summary>
        /// If the object can be selected by the player.
        /// </summary>
        /// <returns>True if the object can be selected.</returns>
        protected virtual bool IsDeselectable()
        {
            return true;
        }

        /// <summary>
        /// Called at the moment the object gets deselected
        /// </summary>
        public virtual void OnDeselection()
        {
            // Empty by default
        }
    }
}