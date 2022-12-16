using System;
using UnityEngine;

namespace Nep.Pool
{
    public interface IObjectPoolManager
    {
        /// <summary>
        /// Number of object pools.
        /// </summary>
        int Count { get; }

        #region Register
        /// <summary>
        /// Create and register a new pool of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IObjectPool<T> RegisterPool<T>() where T : IPoolable;

        /// <summary>
        /// Create and register a new pool of specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ObjectPoolBase RegisterPool(Type type);

        /// <summary>
        /// Create and register a new pool of specified component prefab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        IObjectPool<T> RegisterPool<T>(T obj) where T : Component, IPoolable;

        /// <summary>
        /// Create and register a new pool of specified component prefab.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ObjectPoolBase RegisterPool(Component obj);
        #endregion

        #region Get
        /// <summary>
        /// Get object pool by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IObjectPool<T> GetPool<T>() where T : IPoolable;

        /// <summary>
        /// Get object pool by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ObjectPoolBase GetPool(Type type);

        /// <summary>
        /// Get object pool by component prefab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        IObjectPool<T> GetPool<T>(T obj) where T : Component, IPoolable;

        /// <summary>
        /// Get object pool by component prefab.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ObjectPoolBase GetPool(Component obj);
        #endregion

        #region Check
        /// <summary>
        /// Check whether object pool exists by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool HasPool<T>() where T : IPoolable;

        /// <summary>
        /// Check whether object pool exists by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool HasPool(Type type);

        /// <summary>
        /// Check whether object pool exists by component prefab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool HasPool<T>(T obj) where T : Component, IPoolable;

        /// <summary>
        /// Check whether object pool exists by component prefab.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool HasPool(Component obj);
        #endregion
    }
}