using System;
using UnityEngine;

namespace NekoLib.Pool
{
    /// <summary>
    /// Prefab pool with default delegates.
    /// <para>Pools instances of a prefab referenced by a <see cref="Component"/> instance.</para>
    /// </summary>
    public class AutoComponentPool<T> : AutoPool<T>, IObjectPool<T> where T : Component, new()
    {
        private T _prefab;

        public AutoComponentPool(T prefab, int capacity = kDefaultCapacity,
            Func<T> create = null, Action<T> onGet = null,
            Action<T> onRelease = null, Action<T> destroy = null) 
            : base(capacity, create, onGet, onRelease, destroy)
        {
            _prefab = prefab;
        }

        protected override T DefaultCreateFunc()
        {
            return GameObject.Instantiate(_prefab);
        }

        protected override void DefaultOnRelease(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        protected override void DefaultOnDestroy(T obj)
        {
            GameObject.Destroy(obj.gameObject);
        }
    }
}