using NekoLib.Events;
using UnityEngine;

namespace NekoLib.Singletons
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ReloadDomain()
        {
            _instance = null;
        }
#endif

        public static T Instance {
            get {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}