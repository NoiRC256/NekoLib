using System;
using System.Collections.Generic;
using UnityEngine;

namespace NekoLib.Pool
{
    public abstract class ObjectPoolBase<T> : IObjectPool<T> where T : class
    {
        protected const int kDefaultCapacity = 30;
        protected const float kDefaultExpireTime = 30f;

        public int Capacity { get; set; }
        public int Count => _objectStack.Count;
        public float LastUseTime { get; set; }
        public float ExpireInterval { get; set; }
        public float ExpireTime => LastUseTime + ExpireInterval;

        public event Action<int> CountChanged;

        private Stack<T> _objectStack = new Stack<T>();

        protected ObjectPoolBase(int capacity = kDefaultCapacity, float expireTime = kDefaultExpireTime)
        {
            Capacity = capacity;
            ExpireInterval = expireTime;
        }

        public virtual void Clear()
        {
            while(_objectStack.Count > 0)
            {
                T obj = _objectStack.Pop();
                if (obj != null) Destroy(obj);
            }
            _objectStack.Clear();
            LastUseTime = Time.time;
            CountChanged?.Invoke(_objectStack.Count);
        }

        public virtual void Clear(int count) 
        {
            int cleared = 0;
            while (_objectStack.Count > 0)
            {
                T obj = _objectStack.Pop();
                if (obj != null) Destroy(obj);
                cleared += 1;
                if (cleared >= count) break;
            }
            CountChanged?.Invoke(_objectStack.Count);
        }

        public virtual T Get()
        {
            T obj;
            if (_objectStack.Count > 0)
            {
                obj = TakeFromPool();
            }
            else
            {
                obj = Create();
            }
            return obj;
        }

        /// <summary>
        /// Release an object into the pool.
        /// <para>The object type must be the pool's accepted type.</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if succeeded</returns>
        public bool Release(object obj)
        {
            if (obj == null || obj.GetType() != typeof(T))
            {
                return false;
            }
            return Release((T)obj);
        }

        /// <summary>
        /// Release an object into the pool.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Release(T obj)
        {
            if (_objectStack.Count >= Capacity)
            {
                Destroy(obj);
            }
            else
            {
                AddToPool(obj);
            }
            return true;
        }

        protected abstract T Create();

        protected virtual void Destroy(T obj)
        {

        }

        protected void AddToPool(T obj)
        {
            _objectStack.Push(obj);
            CountChanged?.Invoke(_objectStack.Count);
        }

        protected T TakeFromPool()
        {
            T obj = _objectStack.Pop();
            LastUseTime = Time.time;
            CountChanged?.Invoke(_objectStack.Count);
            return obj;
        }
    }
}