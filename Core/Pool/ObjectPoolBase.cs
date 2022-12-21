namespace Nap.Pool
{
    public abstract class ObjectPoolBase
    {
        /// <summary>
        /// If true, an object can be simultaneously spawned multiple times.
        /// </summary>
        public virtual bool AllowMultiSpawn { get; }
        /// <summary>
        /// Number of objects in use.
        /// </summary>
        public virtual int CountActive { get; protected set; }
        /// <summary>
        /// Number of objects in the pool.
        /// </summary>
        public virtual int CountInactive { get; protected set; }
        /// <summary>
        /// Last time an object is spawned.
        /// </summary>
        public virtual float LastUseTime { get; protected set; }
        /// <summary>
        /// Time until the pool is automatically cleared.
        /// </summary>
        public virtual float ExpireTime { get; set; }
        /// <summary>
        /// Capacity of the pool.
        /// </summary>
        public virtual int Capacity { get; set; }
    }
}