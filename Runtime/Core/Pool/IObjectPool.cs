using System;
using System.Collections.Generic;
using UnityEngine;

namespace NekoLib.Pool
{
    /// <summary>
    /// Base interface for object pool.
    /// <para>Intended to be assigned to pooled instances so they can directly release themselves into a pool.</para>
    /// </summary>
    public interface IObjectPool
    {
        /// <summary>
        /// Capacity of the pool.
        /// </summary>
        int Capacity { get; }
        /// <summary>
        /// Number of objects in the pool.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Last time an object is spawned.
        /// </summary>
        float LastUseTime { get; }
        /// <summary>
        /// Time at which the pool will be automatically cleared.
        /// </summary>
        float ExpireTime { get; }
        /// <summary>
        /// Time until the pool is automatically cleared.
        /// </summary>
        float ExpireInterval { get; }

        public event Action<int> CountChanged;

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