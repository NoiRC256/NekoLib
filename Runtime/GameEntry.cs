using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NekoLib
{
    /// <summary>
    /// Service locator for game modules.
    /// </summary>
    public class GameEntry
    {
        private static readonly Dictionary<Type, object> _gameModules = new Dictionary<Type, object>();

#if UNITY_EDITOR
        [InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            _gameModules.Clear();
        }
#endif

        /// <summary>
        /// Register a component as a module.
        /// <para>Instantiates a <see cref="GameObject"/> containing a component of the specified type.</para>
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="TInstance"></typeparam>
        public static void RegisterModule<TModule, TInstance>()
            where TModule : class where TInstance : Component
        {
            var obj = new GameObject(typeof(TModule).Name).AddComponent<TInstance>();
            RegisterModule<TModule>(obj);
        }

        /// <summary>
        /// Register an object as a module.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="obj"></param>
        public static void RegisterModule<TModule>(object obj) where TModule : class
        {
            if (_gameModules.ContainsKey(typeof(TModule))) return;
            _gameModules.Add(typeof(TModule), obj);

            if (obj.GetType() == typeof(GameObject))
            {
                GameObject.DontDestroyOnLoad((GameObject)obj);
            }
            else if (typeof(Component).IsAssignableFrom(obj.GetType()))
            {
                GameObject.DontDestroyOnLoad((Component)obj);
            }
        }

        /// <summary>
        /// Get a module of the specified type.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        public static TModule GetModule<TModule>() where TModule : class
        {
            return (TModule)GetModule(typeof(TModule));
        }

        /// <summary>
        /// Get a module of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetModule(Type type)
        {
            if (_gameModules.ContainsKey(type))
            {
                return _gameModules[type];
            }
            return null;
        }
    }
}