using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    public enum ActionType
    {
        MOVE,
        ATTACK,
        SKIP,
        REPAIR,
        DESTROY,
        NONE
    }

    public class ActionManager : Singleton<ActionManager>
    {
        private ActionType activeType = ActionType.ATTACK;

        public static bool HasAction(ObjectBehaviour obj, ActionType type)
        {
            return obj.HasAction(obj, type);
        }

        public static bool IsActive(ActionType type)
        {
            return Instance.activeType == type;
        }

        public static void ActivateType(ActionType type)
        {
            Instance.activeType = type;
        }
    }
}
