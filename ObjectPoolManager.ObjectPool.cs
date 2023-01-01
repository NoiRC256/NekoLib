using System;
using System.Collections.Generic;
using UnityEngine;
using Nap.Pool;
using static UnityEditor.Experimental.GraphView.Port;

namespace Nap
{
    public sealed partial class ObjectPoolManager : IObjectPoolManager
    {
        /// <summary>
        /// Type-safe object pool.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ReferencePool<T> : ObjectPoolBase<T> where T : class, new()
        {
            protected override T Create()
            {
                return new T();
            }
        }

        /// <summary>
        /// Type-safe component pool. Can be used to pool instances of a prefab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ComponentPool<T> : ObjectPoolBase<T> where T : Component
        {
            private T _prefab;

            public ComponentPool(T prefab, int capacity = kDefaultCapacity) : base(capacity)
            {
                _prefab = prefab;
            }

            protected override T Create()
            {
                return GameObject.Instantiate(_prefab);
            }

            protected override void Destroy(T obj)
            {
                GameObject.Destroy(obj.gameObject);
            }
        }

        /// <summary>
        /// Gameobject pool. Can be used to pool instances of a prefab.
        /// </summary>
        private class GameObjectPool : ObjectPoolBase<GameObject>
        {
            private GameObject _prefab;

            public GameObjectPool(GameObject prefab, int capacity = kDefaultCapacity) : base(capacity)
            {
                _prefab = prefab;
            }

            protected override GameObject Create()
            {
                return GameObject.Instantiate(_prefab);
            }

            protected override void Destroy(GameObject obj)
            {
                GameObject.Destroy(obj);
            }
        }
    }
}