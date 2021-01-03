using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar
{
    public interface IServiceListener<S>
    {
        public void OnServiceLoading(S service);

        public void OnServiceUnloading(S service);
    }

    public interface IServiceListener
    {

    }

    /// <summary>
    /// Calles listener after the service was loaded
    /// </summary>
    public class ServiceWaiter
    {

    }
}
