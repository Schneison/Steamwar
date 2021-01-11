using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace Steamwar.Utils
{
    public static class InputUtil
    {
        public static bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
