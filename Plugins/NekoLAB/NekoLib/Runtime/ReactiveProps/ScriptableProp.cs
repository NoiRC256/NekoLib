using UnityEngine;

namespace NekoLib.ReactiveProps
{
    /// <summary>
    /// <see cref="UnityEngine.ScriptableObject"/> that acts as a property wrapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ScriptableProp<T> : ScriptableObject, IReadOnlyProp<T>
    {
        [SerializeField] protected T _value;
        public virtual T Value {
            get => _value;
            set => _value = value;
        }
    }
}