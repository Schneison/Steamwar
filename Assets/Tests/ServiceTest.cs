using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Steamwar;
using System.Linq;
using UnityEditor.VersionControl;

public class ServiceTest
{

    private class Service : IService
    {
        public IEnumerator CleanUp()
        {
            yield return null;
        }

        public IEnumerator Initialize()
        {
            yield return null;
        }
    }

    private class BoardService : Service
    {
    }

    private class ObjectService : Service
    {
    }

    private class AttackService : Service
    {
    }

    private class DamageService : Service
    {
    }

    private class AreaService : Service
    {
    }

    [UnityTest]
    public IEnumerator TestLoading()
    {
        Stack<IEnumerator> coroutines = new Stack<IEnumerator>();
        ServiceOrganizer organizer = new ServiceOrganizer((value)=> coroutines.Push(value));
        var board = organizer.Get<BoardService>().Create(() => new BoardService(), (state) => true);
        var obj = organizer.Get<ObjectService>().Create(() => new ObjectService(), (state) => state == LifecycleState.LOADING, ()=> new ServiceContainer[] { board });
        var attack = organizer.Get<AttackService>().Create(() => new AttackService(), (state) => true, () => new ServiceContainer[] { obj });
        var damage = organizer.Get<DamageService>().Create(() => new DamageService(), (state) => true, () => new ServiceContainer[] { attack });
        organizer.Get<AreaService>().Create(() => new AreaService(), (state) => true, () => new ServiceContainer[] { board });
        var (sorted, cycled) = organizer.CreateDependencies();
        if (cycled.Any())
        {
            //Assert.True();
            Debug.Log(cycled);
        }
        organizer.QueueUpdate(LifecycleState.LOADING);
        while (coroutines.Any())
        {
            IEnumerator coroutine = coroutines.Pop();
            yield return coroutine;
        }
        foreach (var item in sorted)
        {
            Assert.True(item.IsAvailable, $"All services should be loaded. {item.ServiceType} failed");
        }
        organizer.QueueUpdate(LifecycleState.SESSION);
        while (coroutines.Any())
        {
            IEnumerator coroutine = coroutines.Pop();
            yield return coroutine;
        }
        foreach (var item in new ServiceContainer[] { obj, attack, damage })
        {
            Assert.False(item.IsAvailable, $"All services should be unloaded. {item.ServiceType} failed");
        }
        organizer.QueueUpdate(LifecycleState.SHUTTTING_DOWN);
        while (coroutines.Any())
        {
            IEnumerator coroutine = coroutines.Pop();
            yield return coroutine;
        }
        foreach (var item in sorted)
        {
            Assert.False(item.IsAvailable, $"All services should be unloaded. {item.ServiceType} failed");
        }
        yield return null;
    }
}
