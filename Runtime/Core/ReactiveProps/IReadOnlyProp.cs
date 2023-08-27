namespace NekoLib.ReactiveProps
{
    /// <summary>
    /// Interface for a readonly property wrapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyProp<T>
    {
         public T Value { get; }
    }
}