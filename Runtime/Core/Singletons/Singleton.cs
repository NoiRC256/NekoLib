using UnityEngine;

namespace NekoLib.Singletons
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

#if UNITY_EDITOR
        // Manual domain reload for editor enter play mode options.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitializeOnEnterPlayMode()
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