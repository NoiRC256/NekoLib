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
        bool AutoExpand { get; }
        int MinCapacity { get; }
        int MaxCapacity { get; }
        /// <summary>
        /// Pool size. The number of objects in the pool.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Last time the pool is used.
        /// </summary>
        float LastUseTime { get; }
        /// <summary>
        /// If the pool has not been used for this amount of time,
        /// the pool size will be optimized.
        /// </summary>
        float ExpireInterval { get; }
        /// <summary>
        /// At this time, the pool size will be optimized.
        /// </summary>
        float ExpireTime { get; }
        /// <summary>
        /// When the pool size has changed.
        /// </summary>
        public event Action<int> CountChanged;

        /// <summary>
        /// Clear the pool.
        /// </summary>
        void Clear();

        /// <summary>
        /// Unspawn an object back into the pool.
        /// </summary>
        /// <param name="obj"></param>
        bool Push(object obj);
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
        bool Push(T obj);
    }
}