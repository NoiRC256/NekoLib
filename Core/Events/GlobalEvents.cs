namespace Nap.Events
{
    public static class GlobalEvents
    {
        private static readonly EventHub _eventHub = new EventHub();

        public static T Get<T>() where T : EventBase
        {
            return _eventHub.Get<T>();
        }
    }
}
