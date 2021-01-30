using Steamwar.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steamwar
{
    public class ServiceManager : Singleton<ServiceManager>, IGameStateListener
    {
        private static readonly ServiceOrganizer organizer = new ServiceOrganizer((coroutine)=>{
            Instance.StartCoroutine(coroutine);
        });

        public static ServiceContainer<S> Get<S>() where S : class, IService
        {
            return organizer.Get<S>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            organizer.CreateDependencies();
        }

        public void Update()
        {
            organizer.Update();
        }

        public void UpdateState(LifecycleState state)
        {
            organizer.UpdateState(state);
        }
    }
}