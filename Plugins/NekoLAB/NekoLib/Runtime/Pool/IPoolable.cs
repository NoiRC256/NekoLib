namespace NekoLib.Pool
{
    /// <summary>
    /// Interface for poolable instances.
    /// </summary>
    public interface IPoolable
    {
        void OnTakeFromPool();
        void OnReturnToPool();
        void Destroy();
    }
}
