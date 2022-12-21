using System;

namespace Nap.Events
{
    /// <summary>
    /// A wrapper class for <see cref="EventHandler{TEventArgs}"></see>. Allows extension of custom event types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventBase<T> : EventBase where T : EventArgs
    {
        public EventHandler<T> Event { get; set; }

        public void Invoke(object sender, T args)
        {
            Event.Invoke(sender, args);
        }
    }

    /// <summary>
    /// A non-generic base class for <see cref="EventBase{T}"></see>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventBase
    {

    }
}