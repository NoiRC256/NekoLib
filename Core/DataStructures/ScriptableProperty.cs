using UnityEngine;

namespace Nap.DataStructures
{
    public abstract class ScriptableProperty<T> : ScriptableObject, IReadonlyProperty<T>
    {
        [SerializeField] private T _value;
        public virtual T Value {
            get => _value;
            set => _value = value;
        }
    }
}