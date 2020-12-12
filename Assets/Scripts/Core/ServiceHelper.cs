using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar
{
    public static class ServiceHelper
    {
        public static bool Available(this IService service, GameState state)
        {
            GameState serviceState = service.GetState();
            return serviceState == GameState.PERSISTENT || serviceState == state;
        }

        public static bool Available(this IService service)
        {
            return Available(service, GameManager.instance.state);
        }
    }
}
