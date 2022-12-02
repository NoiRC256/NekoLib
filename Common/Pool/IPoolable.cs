namespace NekoSystems.Pool
{
    public interface IPoolable
    {
        void OnTakeFromPool();
        void OnReturnToPool();
    }
}
