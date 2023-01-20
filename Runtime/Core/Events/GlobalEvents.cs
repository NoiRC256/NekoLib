namespace NekoLib.Events
{
    /// <summary>
    /// Service locator for global events.
    /// </summary>
    public static class GlobalEvents
    {
        private static readonly EventHub _eventHub = new EventHub();

        public static T Get<T>() where T : AbstractEventBase, new()
        {
            return _eventHub.Get<T>();
        }
    }
}
