using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    public interface IActionListener
    {
        public void OnActionSelected(ActionType action);
    }
}
