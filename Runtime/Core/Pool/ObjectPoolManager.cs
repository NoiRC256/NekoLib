using System;
using System.Collections.Generic;
using UnityEngine;

namespace NekoLib.Pool
{
    /// <summary>
    /// Manager and service locator for pools.
    /// </summary>
    public class ObjectPoolManager : MonoBehaviour, IObjectPoolManager
    {
        private const int kDefaultCapacity = 10;
        private const int kDefaultMaxCapacity = 30;
        private const float kDefaultExpireInterval = 30f;
        private const float kDefaultTickInterval = 15f;

        [SerializeField] private int _defaultCapacity = kDefaultCapacity;
        [SerializeField] private bool _defaultAutoExpand = true;
        [SerializeField] private int _defaultMaxCapacity = kDefaultMaxCapacity;
        [SerializeField] private float _defaultExpireInterval = kDefaultExpireInterval;
        [SerializeField] private float _tickInterval = kDefaultTickInterval;

        private List<IObjectPool> _pools = new List<IObjectPool>();
        private Dictionary<Type, IObjectPool> _referencePools = new Dictionary<Type, IObjectPool>();
        private Dictionary<object, IObjectPool> _prefabPools = new Dictionary<object, IObjectPool>();
        private float _timer;
        public float TickInterval {
            get => _tickInterval;
            set => _tickInterval = value;
        }
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

        #region Create Pool
        public IObjectPool<T> CreatePool<T>() where T : class, new()
        {
            ReferencePool<T> pool = new ReferencePool<T>(capacity: _defaultCapacity,
                autoExpand: _defaultAutoExpand,
                maxCapacity: kDefaultMaxCapacity,
                expireInterval: kDefaultExpireInterval)
            { ExpireInterval = _defaultExpireInterval };
            _referencePools.Add(typeof(T), pool);
            _pools.Add(pool);
            return pool;
        }

        public IObjectPool<T> CreatePool<T>(T obj) where T : Component
        {
            if (obj == null)
                throw new Exception("Object is invalid: null object.");

            IObjectPool<T> pool = new ComponentPool<T>(obj, capacity: _defaultCapacity,
                autoExpand: _defaultAutoExpand,
                maxCapacity: kDefaultMaxCapacity,
                expireInterval: kDefaultExpireInterval)
            { ExpireInterval = _defaultExpireInterval };
            _prefabPools.Add(obj, pool);
            _pools.Add(pool);
            return pool;
        }
        #endregion

        #region Get Pool
        public IObjectPool<T> GetPool<T>() where T : class, new()
        {
            if (HasPool<T>()) return (IObjectPool<T>)_referencePools[typeof(T)];
            else return CreatePool<T>();
        }

        public IObjectPool<T> GetPool<T>(T obj) where T : Component
        {
            if (obj == null)
                throw new Exception("Object is invalid: null object.");

            if (HasPool<T>(obj)) return (IObjectPool<T>)_prefabPools[obj];
            else return CreatePool<T>(obj);
        }
        #endregion

        #region Check Pool
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