using UnityEngine;

namespace Nap.Pool
{
    public interface IObjectPool<T>
    {
        /// <summary>
        /// If true, an object can be simultaneously spawned multiple times.
        /// </summary>
        bool AllowMultiSpawn { get; }
        /// <summary>
        /// Number of objects in use.
        /// </summary>
        int CountActive { get; }
        /// <summary>
        /// Number of objects in the pool.
        /// </summary>
        int CountInactive { get; }
        /// <summary>
        /// Last time an object is spawned.
        /// </summary>
        float LastUseTime { get; }
        /// <summary>
        /// Time until the pool is automatically cleared.
        /// </summary>
        float ExpireTime { get; }
        /// <summary>
        /// Capacity of the pool.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Clear the pool.
        /// </summary>
        void Clear();

        /// <summary>
        /// Spawn an object from the pool.
        /// </summary>
        /// <returns></returns>
        T Get();

        /// <summary>
        /// Unspawn an object back into the pool.
        /// </summary>
        /// <param name="obj"></param>
        void Release(T obj);
    }
}