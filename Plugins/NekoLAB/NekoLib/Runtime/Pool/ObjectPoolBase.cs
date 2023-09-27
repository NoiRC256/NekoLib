using System;
using System.Collections.Generic;
using UnityEngine;

namespace NekoLab.Pool
{
    public abstract class ObjectPoolBase<T> : IObjectPool<T> where T : class
    {
        protected const int kDefaultCapacity = 10;
        protected const int kDefaultMaxCapacity = 30;
        protected const float kDefaultExpireInterval = 30f;

        public int Capacity { get; set; }
        public bool AutoExpand { get; set; }
        public int MinCapacity { get; set; }
        public int MaxCapacity { get; set; }
        public int Count => _objectStack.Count;
        public float LastUseTime { get; set; }
        public float ExpireInterval { get; set; }
        public float ExpireTime => LastUseTime + ExpireInterval;
        public event Action<int> CountChanged;

        private Stack<T> _objectStack = new Stack<T>();

        protected ObjectPoolBase(int capacity = kDefaultCapacity, bool autoExpand = true, int maxCapacity = kDefaultMaxCapacity,
            float expireInterval = kDefaultExpireInterval)
        {
            Capacity = capacity;
            AutoExpand = autoExpand;
            MinCapacity = Capacity;
            MaxCapacity = MaxCapacity;
            LastUseTime = Time.time;
            ExpireInterval = expireInterval;
        }

        public virtual void Clear()
        {
            while (_objectStack.Count > 0)
            {
                T obj = _objectStack.Pop();
                if (obj != null) Destroy(obj);
            }
            _objectStack.Clear();
            LastUseTime = Time.time;
            Capacity = MinCapacity;
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
        public bool Push(object obj)
        {
            if (obj == null || obj.GetType() != typeof(T))
            {
                return false;
            }
            return Push((T)obj);
        }

        /// <summary>
        /// Release an object into the pool.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Push(T obj)
        {
            if (_objectStack.Count >= Capacity)
            {
                if (AutoExpand && Capacity < MaxCapacity)
                {
                    Capacity += 1;
                    AddToPool(obj);
                }
                else Destroy(obj);
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