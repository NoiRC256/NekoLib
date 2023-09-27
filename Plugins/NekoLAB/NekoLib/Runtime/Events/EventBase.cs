using System;

namespace NekoLab.Events
{
    public abstract class EventBase : IEvent
    {
        public event Action Action;

        public void Invoke()
        {
            Action?.Invoke();
        }

        public void Clear() => Action = null;
    }

    /// <summary>
    /// A wrapper class for <see cref="Action{T}"></see> with one event parameter.
    /// Allows extension of custom event types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventBase<T> : IEvent
    {
        public event Action<T> Action;

        public void Invoke(T args)
        {
            Action?.Invoke(args);
        }

        public void Clear() => Action = null;
    }

    /// <summary>
    /// A wrapper class for <see cref="Action{T1, T2}"></see> with two event parameters.
    /// Allows extension of custom event types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventBase<T1, T2> : IEvent
    {
        public event Action<T1, T2> Action;

        public void Invoke(T1 args1, T2 args2)
        {
            Action?.Invoke(args1, args2);
        }

        public void Clear() => Action = null;
    }

    public interface IEvent
    {
        void Clear();
    }
}