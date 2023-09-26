using UnityEngine;

namespace NekoLib.Pool
{
    /// <summary>
    /// Type-safe component pool. Can be used to pool instances of a prefab.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComponentPool<T> : ObjectPoolBase<T> where T : Component
    {
        private T _prefab;

        public ComponentPool(T prefab, int capacity = kDefaultCapacity, bool autoExpand = true,
            int maxCapacity = kDefaultMaxCapacity,
            float expireInterval = kDefaultExpireInterval)
            : base(capacity, autoExpand, maxCapacity, expireInterval)
        {
            _prefab = prefab;
        }

        protected override T Create()
        {
            T obj = GameObject.Instantiate(_prefab);
            return obj;
        }

        protected override void Destroy(T obj)
        {
            GameObject.Destroy(obj.gameObject);
        }
    }
}
