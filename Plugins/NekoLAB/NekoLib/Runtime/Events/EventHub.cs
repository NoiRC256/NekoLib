using System;
using System.Collections.Generic;

namespace NekoLab.Events
{
    /// <summary>
    /// Service locator for event instances.
    /// Uses strongly-typed event classes to identify events, avoiding string id's.
    /// <para>There can only be one instance of each event type in one event hub.</para>
    /// </summary>
    public class EventHub
    {
        private Dictionary<Type, IEvent> _events = new Dictionary<Type, IEvent>();

        /// <summary>
        /// Get an event of the specified type. If the event does not exist, create it.
        /// </summary>
        /// <returns></returns>
        public T Get<T>() where T : IEvent, new()
        {
            Type type = typeof(T);
            if (_events.ContainsKey(type))
            {
                return (T)_events[type];
            }
            else
            {
                T newEvent = new T();
                _events.Add(type, newEvent);
                return newEvent;
            }
        }

        public void Clear()
        {
            _events.Clear();
        }
    }
}