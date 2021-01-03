using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar
{
    public static class Services
    {
        public static readonly ServiceContainer<Registry> registry = ServiceManager.GetOrCreate(()=>new Registry(), (state)=>state==LifcycleState.SESSION);
    }
}
