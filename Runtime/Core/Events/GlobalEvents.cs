using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine;

namespace NekoLib.Events
{
    /// <summary>
    /// Service locator for global events.
    /// </summary>
    public static class GlobalEvents
    {
        private static EventHub _eventHub = new EventHub();

# if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ReloadDomain()
        {
            _eventHub = new EventHub();
        }
#endif

        public static T Get<T>() where T : IEvent, new()
        {
            return _eventHub.Get<T>();
        }
    }
}
