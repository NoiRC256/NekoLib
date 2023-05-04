using System;
using System.Collections.Generic;
using UnityEngine;

namespace NekoLib
{
    /// <summary>
    /// Service locator.
    /// </summary>
    public static class Modules
    {
        private static readonly Dictionary<Type, object> _modules = new Dictionary<Type, object>();

#if UNITY_EDITOR
        // Domain reload for editor enter play mode options.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnEnterPlayMode()
        {
            _modules.Clear();
        }
#endif

        /// <summary>
        /// Register a Unity component module.
        /// <para>This will instantiate a new <see cref="GameObject"/> containing a new component of the specified type.</para>
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        public static void Register<TModule>() where TModule : Component
        {
            if (_modules.ContainsKey(typeof(TModule))) return;

            var obj = new GameObject(typeof(TModule).Name).AddComponent<TModule>();
            Register<TModule>(obj);
        }

        /// <summary>
        /// Register a Unity Component module.
        /// <para>This will instantiate a new <see cref="GameObject"/> containing a new component of the specified type.</para>
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        public static void Register<TModule, TComponent>()
            where TModule : class where TComponent : Component
        {
            if (_modules.ContainsKey(typeof(TModule))) return;

            var obj = new GameObject(typeof(TModule).Name).AddComponent<TComponent>();
            Register<TModule>(obj);
        }

        /// <summary>
        /// Register a Unity GameObject or Component module.
        /// <para>This will instantiate a new <see cref="GameObject"/> that may contain a new component of the specified type.</para>
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="type"></param>
        public static void Register<TModule>(Type type) where TModule : class
        {
            if (_modules.ContainsKey(typeof(TModule))) return;
            if (!typeof(TModule).IsAssignableFrom(type)) return;  

            if (type == typeof(GameObject))
            {
                GameObject go = new GameObject(type.ToString());
                GameObject.DontDestroyOnLoad(go);
                Register<TModule>(go);
            }
            else if (typeof(Component).IsAssignableFrom(type))
            {
                Component c = new GameObject(type.ToString()).AddComponent(type);
                GameObject.DontDestroyOnLoad(c.gameObject);
                Register<TModule>(c);
            }
        }

        /// <summary>
        /// Register an object as a module.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="obj"></param>
        public static void Register<TModule>(object obj) where TModule : class
        {
            if (obj == null) return;
            if (_modules.ContainsKey(typeof(TModule))) return;
            if (!typeof(TModule).IsAssignableFrom(obj.GetType())) return;

            _modules.Add(typeof(TModule), obj);

            if (obj is GameObject)
            {
                GameObject.DontDestroyOnLoad((GameObject)obj);
            }
            else if (obj is Component)
            {
                Component c = (Component)obj;
                GameObject.DontDestroyOnLoad(c.gameObject);
            }
        }

        /// <summary>
        /// Get a module of the specified type.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        public static TModule Get<TModule>() where TModule : class
        {
            return (TModule)Get(typeof(TModule));
        }

        /// <summary>
        /// Get a module of the specified type.
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