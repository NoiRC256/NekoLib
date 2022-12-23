using UnityEngine;

namespace Nap.Pool
{
    /// <summary>
    /// Base interface for object pool.
    /// <para>Intended to be assigned to pooled instances so they can directly release themselves into a pool.</para>
    /// </summary>
    public interface IObjectPool
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
        /// Unspawn an object back into the pool.
        /// </summary>
        /// <param name="obj"></param>
        bool Release(object obj);
    }

    /// <summary>
    /// Generic interface for object pool. Type-safe object pools should implement this interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectPool<T> : IObjectPool
    {
        /// <summary>
        /// Spawn an object from the pool.
        /// </summary>
        /// <returns></returns>
        T Get();

        /// <summary>
        /// Unspawn an object of the pooled type back into the pool.
        /// </summary>
        /// <param name="obj"></param>
        bool Release(T obj);
    }
}