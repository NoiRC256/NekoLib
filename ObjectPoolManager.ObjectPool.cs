using System;
using System.Collections.Generic;
using UnityEngine;
using NekoSystems.Pool;

namespace NekoSystems
{
    public sealed partial class ObjectPoolManager : IObjectPoolManager
    {
        /// <summary>
        /// Generic object pool.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ReferencePool<T> : ObjectPoolBase, IObjectPool<T> where T : IPoolable
        {
            protected const int kDefaultCapacity = 10;

            private Stack<T> _objectStack = new Stack<T>();

            public override int CountInactive => _objectStack.Count;

            public ReferencePool()
            {
                Capacity = kDefaultCapacity;
            }

            public void Clear()
            {
                for (int i = 0; i < _objectStack.Count; i++)
                {
                    T obj = _objectStack.Pop();
                    if (obj != null) Destroy(obj);
                }
            }

            public T Get()
            {
                T obj;
                if (_objectStack.Count > 0)
                {
                    obj = _objectStack.Pop();
                }
                else
                {
                    obj = Create();
                }
                obj.OnTakeFromPool();
                CountActive += 1;
                return obj;
            }

            public void Release(T obj)
            {
                if (_objectStack.Count >= Capacity)
                {
                    Destroy(obj);
                }
                else
                {
                    _objectStack.Push(obj);
                    obj.OnReturnToPool();
                }
                CountActive -= 1;
            }

            protected virtual T Create()
            {
                return Activator.CreateInstance<T>();
            }

            protected virtual void Destroy(T obj)
            {
            }
        }

        /// <summary>
        /// Object pool that pools components. Can be used to pool instances of a prefab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ComponentPool<T> : ReferencePool<T>, IObjectPool<T> where T : Component, IPoolable
        {
            private T _component;

            public ComponentPool(T component) : base()
            {
                _component = component;
            }

            protected override T Create()
            {
                return GameObject.Instantiate(_component);
            }

            protected override void Destroy(T obj)
            {
                GameObject.Destroy(obj.gameObject);
            }
        }
    }
}