using System;

namespace NekoLib.DataStructures
{
    /// <summary>
    /// Interface for property that holds a value and exposes an event for value change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindableProperty<T>
    {
        public T Value { get; set; }
        public event Action<T> ValueChanged;
    }
}