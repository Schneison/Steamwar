using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Steamwar
{
    public interface IMouseListener
    {
        bool MouseDown();

        bool MouseUp();

        bool MouseMove(Vector2 mousePosition, Vector2 lastMouse);
    }
}

