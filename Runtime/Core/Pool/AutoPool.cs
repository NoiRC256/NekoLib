using System;
using System.Collections.Generic;

namespace NekoLib.Pool
{
    /// <summary>
    /// Generic object pool where behaviour of pooled objects can be defined by delegates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoPool<T> : ObjectPoolBase<T> where T : class, new()
    {
        private Stack<T> _objectStack = new Stack<T>();
        private Func<T> _create;
        private Action<T> _onGet;
        private Action<T> _onRelease;
        private Action<T> _destroy;

        public AutoPool(int capacity = kDefaultCapacity,
            Func<T> create = null, Action<T> onGet = null,
            Action<T> onRelease = null, Action<T> destroy = null) : base(capacity)
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

        public override T Get()
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
            _onGet.Invoke(obj);
            return obj;
        }

        public override bool Release(T obj)
        {
            if (_objectStack.Count >= Capacity)
            {
                Destroy(obj);
            }
            else
            {
                AddToPool(obj);
                _onRelease.Invoke(obj);
            }
            return true;
        }

        protected override T Create()
        {
            return _create.Invoke();
        }

        protected override void Destroy(T obj)
        {
            _destroy.Invoke(obj);
        }
    }
}