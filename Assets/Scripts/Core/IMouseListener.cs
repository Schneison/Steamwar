using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Steamwar
{
    /// <summary>
    /// Helper interface that handles mouse events. Can be added to the InputHandler and get called if mouse events occur.
    /// </summary>
    public interface IMouseListener
    {
        /// <summary>
        /// Gets called if a mouse button gets pressed.
        /// </summary>
        /// <returns>True if no other listener should be called after this one.</returns>
        bool MouseDown();

        /// <summary>
        /// Gets called if a mouse button gets released.
        /// </summary>
        /// <returns>True if no other listener should be called after this one.</returns>
        bool MouseUp();

        /// <summary>
        /// Gets called if a mouse moves.
        /// </summary>
        /// <returns>True if no other listener should be called after this one.</returns>
        bool MouseMove(Vector2 mousePosition, Vector2 lastMouse);
    }
}

