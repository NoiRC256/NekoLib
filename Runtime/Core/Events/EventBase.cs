using System;

namespace NekoLib.Events
{
    public abstract class EventBase : IEvent
    {
        public event Action Event;

        public void Invoke()
        {
            Event?.Invoke();
        }

        public void Clear() => Event = null;
    }

    /// <summary>
    /// A wrapper class for <see cref="Action{T}"></see> with one event parameter.
    /// Allows extension of custom event types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventBase<T> : IEvent
    {
        public event Action<T> Event;

        public void Invoke(T args)
        {
            Event?.Invoke(args);
        }

        public void Clear() => Event = null;
    }

    /// <summary>
    /// A wrapper class for <see cref="Action{T1, T2}"></see> with two event parameters.
    /// Allows extension of custom event types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventBase<T1, T2> : IEvent
    {
        public event Action<T1, T2> Event;

        public void Invoke(T1 args1, T2 args2)
        {
            Event?.Invoke(args1, args2);
        }

        public void Clear() => Event = null;
    }

    public interface IEvent
    {
        void Clear();
    }
}