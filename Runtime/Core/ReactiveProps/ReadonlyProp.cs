namespace NekoLib.ReactiveProps
{
    /// <summary>
    /// A readonly property wrapper.
    /// Intended to be a dummy concrete implementation of <see cref="IReadOnlyProp{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ReadonlyProp<T> : IReadOnlyProp<T>
    {
        public T Value { get; }

        public ReadonlyProp(T value)
        {
            Value = value;
        }
    }
}