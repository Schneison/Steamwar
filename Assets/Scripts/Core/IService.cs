using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar
{
    public interface IService
    {
        /// <returns>The state in that this service is available.</returns>
        GameState GetState();

        bool Available();
    }
}
