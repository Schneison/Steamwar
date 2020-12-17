using UnityEngine;
using System.Collections;
using Steamwar.Interaction;
using Steamwar.Utils;

namespace Steamwar.Renderer
{
    public class SessionRenderer : Singleton<SessionRenderer>
    {
        internal SelectionManager selection;

        void Start()
        {
            selection = GetComponent<SelectionManager>();
        }
    }
}
