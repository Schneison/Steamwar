using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar
{
    /// <summary>
    /// Services are called at the loading of a session to create the tilemaps, objects and other game stuff.
    /// </summary>
    public interface IService
    {
        IEnumerator Initialize();

        IEnumerator CleanUp();
    }

    public static class ServiceExtensions
    {
        public static S Get<S>(this S service) where S: class, IService
        {
            return service.Container().Service;
        }

        public static ServiceContainer<S> Container<S>(this S service) where S : class, IService
        {
            return ServiceManager.Get<S>();
        }

    }
}
