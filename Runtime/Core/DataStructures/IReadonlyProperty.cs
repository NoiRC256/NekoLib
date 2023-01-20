namespace NekoLib.DataStructures
{
    /// <summary>
    /// Interface for a readonly property wrapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadonlyProperty<T>
    {
         public T Value { get; }
    }
}