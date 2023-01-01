using System;
using UnityEngine;

namespace Nap.DataStructures
{
    /// <summary>
    /// Data class that holds a value and exposes events for value change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class BindableProperty<T> : IBindableProperty<T> where T : struct
    {
        public virtual T Value {
            get => _value;
            set {
                if (!_value.Equals(value))
                {
                    _value = value;
                    OnValueChange();
                }
            }
        }

        public event Action<T> ValueChanged;

        [SerializeField] protected T _value;

        public BindableProperty(T value)
        {
            Value = value;
        }

        protected virtual void OnValueChange()
        {
            ValueChanged?.Invoke(Value);
        }
    }
}