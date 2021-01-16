using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar
{
    public enum ServiceState
    {
        None,
        Uninitialized,
        Uninitializing,
        Initializing,
        Initialized,
        ShutDown
    }
}
