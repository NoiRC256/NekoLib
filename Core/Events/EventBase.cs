using System;

namespace Nap.Events
{
    /// <summary>
    /// A wrapper class for <see cref="EventHandler{TEventArgs}"></see> with one event parameter.
    /// Allows extension of custom event types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventBase<T> : EventBase
    {
        public Action<T> Event { get; set; }

        public void Invoke(T args)
        {
            Event?.Invoke(args);
        }
    }

    /// <summary>
    /// A wrapper class for <see cref="EventHandler{TEventArgs}"></see> with two event parameters.
    /// Allows extension of custom event types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventBase<T1, T2> : EventBase
    {
        public Action<T1, T2> Event { get; set; }

        public void Invoke(T1 args1, T2 args2)
        {
            Event?.Invoke(args1, args2);
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