
#define DEBUG
#undef DEBUG

using Steamwar.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Steamwar
{
    public class ServiceOrganizer : IGameStateListener
    {
        private int updateCalls = 0;


        private readonly IDictionary<Type, ServiceContainer> containers = new Dictionary<Type, ServiceContainer>();
        private readonly HashSet<ServiceContainer> sorted = new HashSet<ServiceContainer>();
        private readonly Action<IEnumerator> coroutineRunner;
        private bool updates = false;
        private bool dirty = false;
        private bool needUpdate = false;


        public ServiceOrganizer(Action<IEnumerator> coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }

        public ServiceContainer<S> Get<S>() where S : class, IService
        {
            if (containers.ContainsKey(typeof(S)))
            {
                return (ServiceContainer<S>)containers[typeof(S)];
            }
            ServiceContainer<S> container = new ServiceContainer<S>();
            containers[typeof(S)] = container;
            return container;
        }

        public (IEnumerable<ServiceContainer> sorted, IEnumerable<ServiceContainer> cycled) CreateDependencies()
        {
            TopologicalSorter<ServiceContainer> sorter = new TopologicalSorter<ServiceContainer>();
            foreach (var item in containers)
            {
                ServiceContainer[] containers = item.Value.GetDependencies();
                if(containers.Length == 0)
                {
                    sorter.Add(item.Value);
                }
                else
                {
                    sorter.Add(item.Value, containers);
                }
            }
            var a = sorter.Sort();
            sorted.UnionWith(a.sorted);
            return a;

            //unloaded.UnionWith(containers.Values);
        }

        public void QueueUpdate(LifecycleState lifeState = LifecycleState.NONE)
        {
            if (updates)
            {
                return;
            }
            updates = true;
            Coroutine(UpdateServices(lifeState));
        }

        internal void Coroutine(IEnumerator coroutine)
        {
            coroutineRunner(coroutine);
        }

        public IEnumerator UpdateServices(LifecycleState lifeState = LifecycleState.NONE)
        {
            if(lifeState == LifecycleState.NONE)
            {
                lifeState = GameManager.State;
                if(lifeState == LifecycleState.NONE)
                {
                    lifeState = LifecycleState.LOADING;
                }
            }
            dirty = false;
#if DEBUG
            int callIndex = updateCalls;
            Debug.Log($"Start update services {callIndex}.");
#endif
            updateCalls++;
            foreach (ServiceContainer container in this.sorted)
            {
                bool available = container.IsAvailable;
                yield return container.UpdateState(lifeState);
                bool state = container.IsAvailable;
                if (available)
                {
                    if (!state)
                    {
                        dirty = true;
                    }
                }
                else if (state)
                {
                    dirty = true;
                }
            }
            if (dirty)
            {
                yield return UpdateServices(lifeState);
            }
            updateCalls--;
#if DEBUG
            Debug.Log($"End update services {callIndex}.");
#endif
            if (updateCalls == 0)
            {
                updates = false;
                foreach(ServiceContainer container in this.sorted)
                {
                    if (!container.IsAvailable)
                    {
                        Debug.Log($"Failed to load service {nameof(container.ServiceType)}.");
                        continue;
                    }
                    yield return (container?.InternalService as IFinishService)?.Finish();
                }
            }
        }

        public void UpdateState(LifecycleState state)
        {
            needUpdate = true;
        }

        public void Update()
        {
            if (needUpdate)
            {
                QueueUpdate();
                needUpdate = false;
            }
        }
    }

    public class ServiceContainer<S> : ServiceContainer where S : class, IService
    {
        private ServiceHandler<S> handler;

        public ServiceContainer<T> Create<T>(Predicate<LifecycleState> gameStateValidator) where T : Singleton<T>, S
        {
            return Create(() => Singleton<T>.Instance, gameStateValidator, () => new ServiceContainer[0]) as ServiceContainer<T>;
        }

        public ServiceContainer<T> Create<T>(Predicate<LifecycleState> gameStateValidator, Func<ServiceContainer[]> dependencies) where T : Singleton<T>, S
        {
            return Create(() => Singleton<T>.Instance, gameStateValidator, dependencies) as ServiceContainer<T>;
        }

        public ServiceContainer<S> Create(Func<S> factory, Predicate<LifecycleState> gameStateValidator)
        {
            return Create(factory, gameStateValidator, () => new ServiceContainer[0]);
        }

        public ServiceContainer<S> Create(Func<S> factory, Predicate<LifecycleState> gameStateValidator, Func<ServiceContainer[]> dependencies)
        {
            this.handler = new ServiceHandler<S>(factory, gameStateValidator, dependencies);
            return this;
        }

        public override ServiceContainer[] GetDependencies()
        {
            return handler.GetDependencies();
        }

        public bool DependenciesLoaded()
        {
            return handler.DependenciesLoaded();
        }

        public S Service => handler.Service;

        public override IService InternalService => Service;

        public override Type ServiceType => typeof(S);

        public override ServiceState State => handler == null ? ServiceState.None : handler.State;

        public override bool IsLoadable => handler.IsLoadable;

        public override bool IsAvailable => State == ServiceState.Initialized;

        public override bool IsLoading => State == ServiceState.Initializing;

        public override IEnumerator UpdateState(LifecycleState state) => handler.UpdateState(state);

        public void Listen(IServiceListener<S> listener) => handler.Listen(listener);

        public static implicit operator S(ServiceContainer<S> value)
        {
            return value.Service;
        }
    }

    public abstract class ServiceContainer
    {
        public abstract Type ServiceType
        {
            get;
        }

        public abstract ServiceState State
        {
            get;
        }

        public abstract bool IsLoading
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

        public abstract IService InternalService
        {
            get;
        }

        public abstract IEnumerator UpdateState(LifecycleState state);

        public abstract ServiceContainer[] GetDependencies();
    }

    internal class ServiceHandler<S> where S : class, IService
    {
        private readonly List<IServiceListener<S>> listeners = new List<IServiceListener<S>>();
        private readonly Func<ServiceContainer[]> dependencyFactory;
        private readonly Func<S> factory;
        private readonly Predicate<LifecycleState> gameStateValidator;
        private readonly bool persistent;
        private ServiceContainer[] dependencies;
        private ServiceState _state = ServiceState.Uninitialized;
        private S service;

        public ServiceHandler(Func<S> factory, Predicate<LifecycleState> gameStateValidator, Func<ServiceContainer[]> dependencies)
        {
            this.factory = factory;
            this.gameStateValidator = gameStateValidator;
            this.persistent = gameStateValidator(LifecycleState.PERSISTENT);
            this.dependencyFactory = dependencies;
        }

        public ServiceContainer[] GetDependencies()
        {
            if (dependencies == null)
            {
                dependencies = dependencyFactory();
            }
            return dependencies;
        }

        public bool DependenciesLoaded()
        {
            return GetDependencies().All((dependency) => dependency.IsAvailable);
        }

        public S Service
        {
            get
            {
                Assert.IsNotNull(service, "Service was not available at the call of this property. Please call 'IsAvailable' before you try to get the service object.");
                return service;
            }
        }

        public Type ServiceType => typeof(S);

        public ServiceState State => _state;

        public bool IsAvailable => _state == ServiceState.Initialized;

        public bool IsLoading => _state == ServiceState.Initializing;

        public bool IsLoadable => persistent || gameStateValidator(GameManager.State);

        private bool ForceUnloading(LifecycleState state)
        {
            return state == LifecycleState.SHUTTTING_DOWN || state == LifecycleState.NONE;
        }

        public IEnumerator UpdateState(LifecycleState state)
        {
            bool valid = (persistent || gameStateValidator(state))
                && !ForceUnloading(state)
                && DependenciesLoaded();
            if (IsAvailable)
            {
                if (valid)
                {
                    yield return null;
                }
                else
                {

                    _state = ServiceState.Uninitializing;
                    yield return UninitializeService();
                    service = default;
                    yield return null;
                }
            }
            else if (valid)
            {
                service = factory();
                _state = ServiceState.Initializing;
                yield return InitializeService();
            }
        }

        private IEnumerator InitializeService()
        {
#if DEBUG
            Debug.Log($"Start initialize {typeof(S)}");
#endif
            yield return service.Initialize();
            _state = ServiceState.Initialized;
            listeners.ForEach((listener) => listener.OnServiceLoading(service));
#if DEBUG
            Debug.Log($"End initialize {typeof(S)}");
#endif
        }

        private IEnumerator UninitializeService()
        {
#if DEBUG
            Debug.Log($"Start uninitialize {typeof(S)}");
#endif
            listeners.ForEach((listener) => listener.OnServiceUnloading(service));
            yield return service.CleanUp();
            _state = ServiceState.Uninitialized;
#if DEBUG
            Debug.Log($"End uninitialize {typeof(S)}");
#endif
        }

        public void Listen(IServiceListener<S> listener)
        {
            listeners.Add(listener);
        }

        public static implicit operator S(ServiceHandler<S> value)
        {
            return value.Service;
        }
    }
}
