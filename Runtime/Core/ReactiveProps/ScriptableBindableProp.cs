using System;
using System.Diagnostics;

namespace NekoLib.ReactiveProps
{
    /// <summary>
    /// <see cref="UnityEngine.ScriptableObject"/> that acts as a property wrapper and exposes an event for value change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScriptableBindableProp<T> : ScriptableProp<T>, IBindableProp<T>, IReadOnlyProp<T>
        where T : struct
    {
        public override T Value {
            set {
                if (!_value.Equals(value))
                {
                    _value = value;
                    OnValueChange();
                }
            }
        }

        public event Action<T> ValueChanged;

        protected virtual void OnValueChange()
        {
            ValueChanged?.Invoke(Value);
        }

        private void OnValidate()
        {
            Value = _value;
            OnValueChange();
        }
    }
}
