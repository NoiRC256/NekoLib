namespace NekoLib.Pool
{
    /// <summary>
    /// Type-safe object pool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReferencePool<T> : ObjectPoolBase<T> where T : class, new()
    {
        public ReferencePool(int capacity = kDefaultCapacity, bool autoExpand = true,
            int maxCapacity = kDefaultMaxCapacity,
            float expireInterval = kDefaultExpireInterval)
            : base(capacity, autoExpand, maxCapacity, expireInterval)
        {
        }

        protected override T Create()
        {
            return new T();
        }
    }
}
