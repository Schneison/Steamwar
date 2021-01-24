using Steamwar.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Turns
{
    public interface TurnHandler : ITurnListener
    {
        public abstract bool CanProgress();
    }
}
