using System;
using System.Collections.Generic;
using UnityEngine;
using NekoSystems.Pool;

namespace NekoSystems
{
    public sealed partial class ObjectPoolManager : MonoBehaviour, IObjectPoolManager
    {
        private const int kDefaultCapacity = int.MaxValue;
        private const float kDefaultExpireTime = -1f;

        private static Dictionary<Type, ObjectPoolBase> _referencePools;
        private static Dictionary<object, ObjectPoolBase> _componentPools;

        public int Count { get; private set; }

        private void Awake()
        {
            _referencePools = new Dictionary<Type, ObjectPoolBase>();
            _componentPools = new Dictionary<object, ObjectPoolBase>();
        }

        public IObjectPool<T> RegisterPool<T>() where T : IPoolable
        {
            ReferencePool<T> pool = new ReferencePool<T>();
            _referencePools.Add(typeof(T), pool);
            return (IObjectPool<T>)_referencePools[typeof(T)];
        }

        public ObjectPoolBase RegisterPool(Type type)
        {
            if (!typeof(IPoolable).IsAssignableFrom(type))
                throw new GameFrameworkException(String.Format("Object type '{0}' is invalid", type.FullName));

            Type poolType = typeof(ReferencePool<>).MakeGenericType(type);
            ObjectPoolBase pool = (ObjectPoolBase)Activator.CreateInstance(poolType);
            _referencePools.Add(type, pool);
            return pool;
        }

        public IObjectPool<T> RegisterPool<T>(T obj) where T : Component, IPoolable
        {
            if (obj == null) 
                throw new GameFrameworkException("Object is invalid.");

            ComponentPool<T> pool = new ComponentPool<T>(obj);
            _componentPools.Add(obj, pool);
            return pool;
        }

        public ObjectPoolBase RegisterPool(Component obj)
        {
            if (obj == null) 
                throw new GameFrameworkException("Object is invalid.");
            Type type = obj.GetType();
            if (!typeof(IPoolable).IsAssignableFrom(type))
                throw new GameFrameworkException(String.Format("Object type '{0}' is invalid", type.FullName));

            Type poolType = typeof(ComponentPool<>).MakeGenericType(type);
            ObjectPoolBase pool = (ObjectPoolBase)Activator.CreateInstance(poolType);
            _componentPools.Add(obj, pool);
            return pool;
        }

        public IObjectPool<T> GetPool<T>() where T : IPoolable
        {
            if (HasPool<T>()) return (IObjectPool<T>)_referencePools[typeof(T)];
            else return RegisterPool<T>();
        }

        public ObjectPoolBase GetPool(Type type)
        {
            if (!typeof(IPoolable).IsAssignableFrom(type))
                throw new GameFrameworkException(String.Format("Object type '{0}' is invalid", type.FullName));

            if (HasPool(type)) return _referencePools[type];
            else return RegisterPool(type);
        }

        public IObjectPool<T> GetPool<T>(T obj) where T : Component, IPoolable
        {
            if (obj == null) 
                throw new GameFrameworkException("Object is invalid.");
            if (HasPool<T>(obj)) return (IObjectPool<T>)_componentPools[obj];
            else return RegisterPool<T>(obj);
        }

        public ObjectPoolBase GetPool(Component obj)
        {
            if (obj == null) 
                throw new GameFrameworkException("Object is invalid.");
            if (!typeof(IPoolable).IsAssignableFrom(obj.GetType()))
                throw new GameFrameworkException(String.Format("Object type '{0}' is invalid", obj.GetType().FullName));

            if (HasPool(obj)) return _componentPools[obj];
            else return RegisterPool(obj);
        }

        public bool HasPool<T>() where T : IPoolable
        {
            var type = typeof(T);
            return _referencePools.ContainsKey(type);
        }

        public bool HasPool(Type type)
        {
            return _referencePools.ContainsKey(type);
        }

        public bool HasPool<T>(T obj) where T : Component, IPoolable
        {
            return _componentPools.ContainsKey(obj);
        }

        public bool HasPool(Component obj)
        {
            return _componentPools.ContainsKey(obj);
        }
    }
}