using UnityEngine;

namespace NekoLab.Events
{
    /// <summary>
    /// Service locator for global events.
    /// </summary>
    public static class GlobalEvents
    {
        private static EventHub _eventHub = new EventHub();

# if UNITY_EDITOR
        // Manual domain reload for editor enter play mode options.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitializeOnEnterPlayMode()
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
