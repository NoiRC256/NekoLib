namespace Nap.Pool
{
    public interface IPoolable
    {
        void OnTakeFromPool();
        void OnReturnToPool();
        void Destroy();
    }
}
