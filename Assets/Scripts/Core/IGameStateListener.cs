using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar
{
    public interface IGameStateListener
    {
        public void OnState(LifcycleState state);
    }
}
