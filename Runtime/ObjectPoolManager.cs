using System;
using System.Collections.Generic;
using UnityEngine;
using NekoLib.Pool;
using Unity.VisualScripting;

namespace NekoLib
{
    /// <summary>
    /// Manager and service locator for pools.
    /// </summary>
    public sealed partial class ObjectPoolManager : MonoBehaviour, IObjectPoolManager
    {
        private const int kDefaultCapacity = 30;
        private const float kDefaultExpireTime = 30f;
        private const float kDefaultTickInterval = 15f;

        private List<IObjectPool> _pools = new List<IObjectPool>();
        private Dictionary<Type, IObjectPool> _referencePools = new Dictionary<Type, IObjectPool>();
        private Dictionary<object, IObjectPool> _prefabPools = new Dictionary<object, IObjectPool>();
        private float _timer;

        public float TickInterval { get; set; }
        public int Count { get; private set; }
        public Dictionary<Type, IObjectPool> ReferencePools => _referencePools;
        public Dictionary<object, IObjectPool> PrefabPools => _prefabPools;
        public float Timer => _timer;

        #region MonoBehaviour
        private void Awake()
        {
            _pools = new List<IObjectPool>();
            _referencePools = new Dictionary<Type, IObjectPool>();
            _prefabPools = new Dictionary<object, IObjectPool>();
            TickInterval = kDefaultTickInterval;
            _timer = TickInterval;
        }

        private void LateUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer >= TickInterval)
            {
                _timer = 0f;
                // Clear pools that reached capacity or reached expire time since last use.
                for (int i = 0; i < _pools.Count; i++)
                {
                    IObjectPool pool = _pools[i];
                    if (pool.Count >= pool.Capacity || Time.time >= pool.ExpireTime)
                    {
                        pool.Clear();
                    }
                }
            }
        }
        #endregion

        #region Register
        public IObjectPool<T> RegisterPool<T>() where T : class, new()
        {
            ReferencePool<T> pool = new ReferencePool<T>();
            _referencePools.Add(typeof(T), pool);
            _pools.Add(pool);
            return pool;
        }

        public IObjectPool<T> RegisterPool<T>(T obj) where T : Component
        {
            if (obj == null)
                throw new Exception("Object is invalid: null object.");

            IObjectPool<T> pool = new ComponentPool<T>(obj);
            _prefabPools.Add(obj, pool);
            _pools.Add(pool);
            return pool;
        }
        #endregion

        #region Get
        public IObjectPool<T> GetPool<T>() where T : class, new()
        {
            if (HasPool<T>()) return (IObjectPool<T>)_referencePools[typeof(T)];
            else return RegisterPool<T>();
        }

        public IObjectPool<T> GetPool<T>(T obj) where T : Component
        {
            if (obj == null)
                throw new Exception("Object is invalid: null object.");

            if (HasPool<T>(obj)) return (IObjectPool<T>)_prefabPools[obj];
            else return RegisterPool<T>(obj);
        }
        #endregion

        #region Check
        public bool HasPool<T>() where T : class, new()
        {
            return _referencePools.ContainsKey(typeof(T));
        }

        public bool HasPool<T>(T obj) where T : Component
        {
            return _prefabPools.ContainsKey(obj);
        }
        #endregion
    }
}