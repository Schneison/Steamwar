using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    public interface IPropListener
    {
        /// <summary>
        /// Called for objects that are created by the sector and were not spawned by the player or an other faction. 
        /// 
        /// Gets called after all game systems were initialized so the object can safely create its data and do other actions that are based on these systems.
        /// </summary>
        public void OnPropInit();
    }
}
