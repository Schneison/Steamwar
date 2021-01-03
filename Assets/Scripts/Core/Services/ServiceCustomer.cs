using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar
{
    public class ServiceCustomer<S> : SteamBehaviour, IServiceListener<S> where S : IService
    {

        public virtual void OnServiceLoading(S service)
        {

        }

        public virtual void OnServiceUnloading(S service)
        {

        }

        protected override void OnInit()
        {
            base.OnInit();

        }
    }
}
