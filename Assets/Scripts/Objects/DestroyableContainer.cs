using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Steamwar.Objects {

    public abstract class DestroyableContainer<D, T, S> : ObjectContainer<D, T, S> where D : DestroyableData<T, S>, new() where T : ObjectType where S : DestroyableDataSerializable, new()
    {

    }
}
