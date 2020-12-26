using Steamwar.Interaction;
using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    [Flags]
    public enum ActionType
    {
        None = 0b0,
        Move = 0b1,
        Atttack = 0b10,
        Skip = 0b100,
        Repair = 0b1000,
        Destroy = 0b10000,
        All = 0b11111
    }

    public class ActionManager : Singleton<ActionManager>
    {
        private ActionType activeType = ActionType.Move;

        public static bool HasAction(ObjectBehaviour obj, ActionType type)
        {
            return obj.GetAction().HasFlag(type);
        }

        public static bool IsActive(ActionType type)
        {
            return Instance.activeType == type;
        }

        private bool DeselectTypeInternal()
        {
            if(activeType == ActionType.None)
            {
                return false;
            }
            activeType = ActionType.None;
            SelectionManager.Instance.listeners.OnActionUpdate(ActionType.None);
            return true;
        }

        public static bool DeselectType()
        {
            return Instance.DeselectTypeInternal();
        }

        public static bool ActivateType(ActionType type)
        {
            Instance.activeType = type;
            SelectionManager.Instance.listeners.OnActionUpdate(type);
            return true;
        }
    }
}
