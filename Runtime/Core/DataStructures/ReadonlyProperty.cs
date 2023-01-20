namespace NekoLib.DataStructures
{
    /// <summary>
    /// A readonly property wrapper.
    /// Intended to be a dummy concrete implementation of <see cref="IReadonlyProperty{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ReadonlyProperty<T> : IReadonlyProperty<T>
    {
        public T Value { get; }

        public ReadonlyProperty(T value)
        {
            Value = value;
        }
    }
}