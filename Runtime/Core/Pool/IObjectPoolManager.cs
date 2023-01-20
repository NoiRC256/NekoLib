using UnityEngine;

namespace NekoLib.Pool
{
    /// <summary>
    /// Interface for object pool manager that manages pools for <see cref="IPoolable"/> instances.
    /// </summary>
    public interface IObjectPoolManager
    {
        /// <summary>
        /// Number of pools.
        /// </summary>
        int Count { get; }

        #region Register
        /// <summary>
        /// Create and register a new pool of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IObjectPool<T> RegisterPool<T>() where T : class, new();

        /// <summary>
        /// Create and register a new pool of the specified component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        IObjectPool<T> RegisterPool<T>(T obj) where T : Component;
        #endregion

        #region Get
        /// <summary>
        /// Get a pool by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IObjectPool<T> GetPool<T>() where T : class, new();

        /// <summary>
        /// Get a pool by component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        IObjectPool<T> GetPool<T>(T obj) where T : Component;

        #endregion

        #region Check
        /// <summary>
        /// Check whether pool exists by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool HasPool<T>() where T : class, new();

        /// <summary>
        /// Check whether pool exists by component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool HasPool<T>(T obj) where T : Component;
        #endregion
    }
}