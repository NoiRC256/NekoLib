using System;
using UnityEngine;

namespace Nep.DataStructures
{
    /// <summary>
    /// Data class that holds a value and invokes events when the value is changed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class BindableProperty<T> : IBindableProperty<T> where T : IEquatable<T>
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