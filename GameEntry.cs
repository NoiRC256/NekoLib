using System;
using System.Collections.Generic;

namespace Nap
{
    /// <summary>
    /// Service locator for game modules.
    /// </summary>
    public class GameEntry
    {
        private static readonly Dictionary<Type, object> _gameModules = new Dictionary<Type, object>();

        public static void RegisterModule<T>(object obj) where T : class
        {
            if (_gameModules.ContainsKey(typeof(T))) return;
            _gameModules.Add(typeof(T), obj);
        }

        public static T GetModule<T>() where T : class
        {
            return (T)GetModule(typeof(T));
        }

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