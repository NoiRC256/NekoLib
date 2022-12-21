using System;
using System.Collections.Generic;

namespace Nap.Pool
{
    /// <summary>
    /// Generic object pool where behaviour of pooled objects can be defined by delegates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoPool<T> : IObjectPool<T> where T : class, new()
    {
        protected const int kDefaultCapacity = 10;

        private Stack<T> _objectStack = new Stack<T>();
        private Func<T> _create;
        private Action<T> _onGet;
        private Action<T> _onRelease;
        private Action<T> _destroy;

        public int CountActive { get; private set; }
        public int CountInactive => _objectStack.Count;
        public bool AllowMultiSpawn { get; }
        public float LastUseTime { get; private set; }
        public float ExpireTime { get; set; }
        public int Capacity { get; set; }

        public AutoPool(int capacity = kDefaultCapacity,
            Func<T> create = null, Action<T> onGet = null,
            Action<T> onRelease = null, Action<T> destroy = null)
        {
            Capacity = capacity;
            _create = create == null ? DefaultCreateFunc : create;
            _onGet = onGet == null ? DefaultOnGet : onGet;
            _onRelease = onRelease == null ? DefaultOnRelease : onRelease;
            _destroy = destroy == null ? DefaultOnDestroy : destroy;
        }

        #region Default Delegates
        /// <summary>
        /// Default method called to create an object for the pool.
        /// </summary>
        /// <returns></returns>
        protected virtual T DefaultCreateFunc()
        {
            return new T();
        }

        /// <summary>
        /// Default method called when taking an object from the pool.
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void DefaultOnGet(T obj)
        {

        }

        /// <summary>
        /// Default method called when releasing an object into the pool.
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void DefaultOnRelease(T obj)
        {

        }

        /// <summary>
        /// Default method called when destroying an object.
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void DefaultOnDestroy(T obj)
        {
            obj = null;
        }
        #endregion

        /// <summary>
        /// Clear the pool.
        /// </summary>
        public void Clear()
        {
            for(int i = 0; i < _objectStack.Count; i++)
            {
                T obj = _objectStack.Pop();
                if (obj != null) _destroy(obj);
            }
        }

        /// <summary>
        /// Take an object from the pool.
        /// If no object available, create one.
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            T obj;
            if (_objectStack.Count > 0)
            {
                obj = _objectStack.Pop();
            }
            else
            {
                obj = _create();
            }
            _onGet(obj);
            CountActive += 1;
            return obj;
        }

        /// <summary>
        /// Release an object into the pool.
        /// </summary>
        /// <param name="obj"></param>
        public void Release(T obj)
        {
            if (_objectStack.Count >= Capacity)
            {
                _destroy(obj);
            }
            else
            {
                _objectStack.Push(obj);
                _onRelease(obj);
            }
            CountActive -= 1;
        }
    }
}