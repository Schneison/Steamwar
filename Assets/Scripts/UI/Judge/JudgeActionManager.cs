using Steamwar.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Steamwar.UI
{
    public class JudgeActionManager : SteamBehaviour, IJudgeListener, IActionListener
    {
        public JudgeAction firstContainer;
        public JudgeAction secondContainer;
        public JudgeAction thirdContainer;

        private JudgeAction[] _actions;

        public JudgeAction[] Actions {
            get
            {
                if(_actions == null)
                {
                    _actions = new JudgeAction[] { firstContainer, secondContainer, thirdContainer };
                }
                return _actions;
            }
        }

        public void OnActionSelected(ActionType type)
        {
            foreach(JudgeAction button in Actions)
            {
                if(button.type == type)
                {
                    button.SetExpanded(true);
                }
                else
                {
                    button.SetExpanded(false);
                }
            }
        }

        public void OnJudgeChange(ObjectContainer container)
        {
        }

        public void OnJudgeClear(ObjectContainer container)
        {
            foreach (JudgeAction button in Actions)
            {
                button.Clear();
            }
        }
    }

}
