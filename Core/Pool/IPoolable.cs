namespace Nep.Pool
{
    public interface IPoolable
    {
        void OnTakeFromPool();
        void OnReturnToPool();
    }
}
