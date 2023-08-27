using System;
using System.Collections.Generic;
using UnityEngine;

namespace NekoLib.ServiceLocator
{
    /// <summary>
    /// Service locator.
    /// </summary>
    public static class GameServices
    {
        private static readonly Dictionary<Type, object> _modules = new Dictionary<Type, object>();

#if UNITY_EDITOR
        // Manual domain reload for editor enter play mode options.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnEnterPlayMode()
        {
            _modules.Clear();
        }
#endif

        /// <summary>
        /// Register a Unity component game service.
        /// <para>This will instantiate a new <see cref="GameObject"/> containing a new component of the specified type.</para>
        /// </summary>
        /// <typeparam name="TGameService"></typeparam>
        public static void Register<TGameService>() where TGameService : Component
        {
            if (_modules.ContainsKey(typeof(TGameService))) return;

            var obj = new GameObject(typeof(TGameService).Name).AddComponent<TGameService>();
            Register<TGameService>(obj);
        }

        /// <summary>
        /// Register a Unity Component game service.
        /// <para>This will instantiate a new <see cref="GameObject"/> containing a new component of the specified type.</para>
        /// </summary>
        /// <typeparam name="TGameService"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        public static void Register<TGameService, TComponent>()
            where TGameService : class where TComponent : Component
        {
            if (_modules.ContainsKey(typeof(TGameService))) return;

            var obj = new GameObject(typeof(TGameService).Name).AddComponent<TComponent>();
            Register<TGameService>(obj);
        }

        /// <summary>
        /// Register a Unity GameObject or Component game service.
        /// <para>This will instantiate a new <see cref="GameObject"/> that may contain a new component of the specified type.</para>
        /// </summary>
        /// <typeparam name="TGameService"></typeparam>
        /// <param name="type"></param>
        public static void Register<TGameService>(Type type) where TGameService : class
        {
            if (_modules.ContainsKey(typeof(TGameService))) return;
            if (!typeof(TGameService).IsAssignableFrom(type)) return;

            if (type == typeof(GameObject))
            {
                GameObject go = new GameObject(type.ToString());
                UnityEngine.Object.DontDestroyOnLoad(go);
                Register<TGameService>(go);
            }
            else if (typeof(Component).IsAssignableFrom(type))
            {
                Component c = new GameObject(type.ToString()).AddComponent(type);
                UnityEngine.Object.DontDestroyOnLoad(c.gameObject);
                Register<TGameService>(c);
            }
        }

        /// <summary>
        /// Register an object as a game service.
        /// </summary>
        /// <typeparam name="TGameService"></typeparam>
        /// <param name="obj"></param>
        public static void Register<TGameService>(object obj) where TGameService : class
        {
            if (obj == null) return;
            if (_modules.ContainsKey(typeof(TGameService))) return;
            if (!typeof(TGameService).IsAssignableFrom(obj.GetType())) return;

            _modules.Add(typeof(TGameService), obj);

            if (obj is GameObject)
            {
                UnityEngine.Object.DontDestroyOnLoad((GameObject)obj);
            }
            else if (obj is Component)
            {
                Component c = (Component)obj;
                UnityEngine.Object.DontDestroyOnLoad(c.gameObject);
            }
        }

        /// <summary>
        /// Get a game service of the specified type.
        /// </summary>
        /// <typeparam name="TGameService"></typeparam>
        /// <returns></returns>
        public static TGameService Get<TGameService>() where TGameService : class
        {
            return (TGameService)Get(typeof(TGameService));
        }

        /// <summary>
        /// Get a game service of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Get(Type type)
        {
            if (_modules.ContainsKey(type))
            {
                return _modules[type];
            }
            return null;
        }
    }
}