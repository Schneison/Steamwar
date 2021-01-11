using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Interaction
{
    public interface ISelectionUIListener
    {
        /// <summary>
        /// Called if an action is selected and the pointer enters an ui element.
        /// </summary>
        /// <param name="data"></param>
        void OnSelectionUIEnter(SelectionData data);

        /// <summary>
        /// Called if an action is selected and the pointer exits an ui element.
        /// </summary>
        void OnSelectionUIExit(SelectionData data);

        public bool CallsMoveOverUI
        {
            get;
        }
    }
}
