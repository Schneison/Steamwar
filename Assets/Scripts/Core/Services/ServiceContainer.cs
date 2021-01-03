using MyBox;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar
{
    public static class ServiceManager
    {
        private static readonly IDictionary<Type, ServiceContainer> containers = new Dictionary<Type, ServiceContainer>();

        public static ServiceContainer<S> GetOrCreate<S>(ServiceContainer<S>.ServiceFactory factory, Predicate<LifcycleState> gameStateValidator) where S : IService
        {
            if (containers.ContainsKey(typeof(S)))
            {
                return Get<S>();
            }
            ServiceContainer<S> container = new ServiceContainer<S>(factory, gameStateValidator);
            containers[typeof(S)] = container;
            return container;
        }

        public static ServiceContainer<S> Get<S>() where S : IService
        {
            return (ServiceContainer<S>) containers[typeof(S)];
        }
    }

    public class ServiceContainer<S> : ServiceContainer, IGameStateListener where S : IService
    {
        public delegate S ServiceFactory();

        private readonly List<IServiceListener<S>> listeners = new List<IServiceListener<S>>();
        private readonly ServiceFactory factory;
        private readonly Predicate<LifcycleState> gameStateValidator;
        private readonly bool persistent;
        private ServiceState _state = ServiceState.Uninitialized;
        private S service;

        public ServiceContainer(ServiceFactory factory, Predicate<LifcycleState> gameStateValidator)
        {
            this.factory = factory;
            this.gameStateValidator = gameStateValidator;
            this.persistent = gameStateValidator(LifcycleState.PERSISTENT);
        }

        public S Service
        {
            get
            {
                Assert.IsNotNull(service, "Service was not available at the call of this property. Please call 'IsAvailable' before you try to get the service object.");
                return service;
            }
        }

        public override ServiceState State
        {
            get => _state;
        }

        public override bool IsAvailable
        {
            get=> _state == ServiceState.Initialized;
        }

        public override bool IsLoadable
        {
            get => persistent || gameStateValidator(GameManager.state);
        }

        private bool ForceUnloading(LifcycleState state)
        {
            return state == LifcycleState.SHUTTTING_DOWN || state == LifcycleState.NONE;
        }

        public override void OnState(LifcycleState state)
        {
            bool valid = (persistent && !ForceUnloading(state)) || gameStateValidator(state);
            if (IsAvailable)
            {
                if (valid)
                {
                    return;
                }
                else
                {
                    listeners.ForEach((listener) => listener.OnServiceUnloading(service));
                    _state = ServiceState.Uninitialized;
                    service = default;
                }
            }
            service = factory();
            _state = ServiceState.Initialized;
            listeners.ForEach((listener) => listener.OnServiceLoading(service));
        }

        public void Listen(IServiceListener<S> listener)
        {
            listeners.Add(listener);
        }

        public static bool operator true(ServiceContainer<S> x) => x.IsAvailable;

        public static bool operator false(ServiceContainer<S> x) => !x.IsAvailable;
    }

    public abstract class ServiceContainer
    {
        public abstract ServiceState State
        {
            get;
        }

        public abstract bool IsAvailable
        {
            get;
        }

        public abstract bool IsLoadable
        {
            get;
        }

        public abstract void OnState(LifcycleState state);
    }
}
