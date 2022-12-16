using System;

namespace Nep.DataStructures
{
    /// <summary>
    /// Interface for property that holds a value and invokes events when the value is changed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindableProperty<T>
    {
        public T Value { get; set; }
        public event Action<T> ValueChanged;
    }
}