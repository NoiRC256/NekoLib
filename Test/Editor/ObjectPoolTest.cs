using NUnit.Framework;
using Nep.Pool;
using UnityEngine;

namespace Nep.Test
{
    public class ObjectPoolTest
    {
        class PooledType : IPoolable
        {
            public void OnReturnToPool()
            {
            }

            public void OnTakeFromPool()
            {
            }
        }

        class PooledMono : MonoBehaviour, IPoolable
        {
            public void OnReturnToPool()
            {
            }

            public void OnTakeFromPool()
            {
            }
        }

        [Test]
        public void RegisterPool()
        {
            var manager = new ObjectPoolManager();

            Assert.IsFalse(manager.HasPool<PooledType>());

            manager.RegisterPool<PooledType>();

            Assert.IsTrue(manager.HasPool<PooledType>());
        }

        [Test]
        public void GetMultiplePools()
        {
            var manager = new ObjectPoolManager();
            var pool = manager.GetPool<PooledType>();

            Assert.NotNull(pool);

            var pool2 = manager.GetPool<PooledType>();
            var pool3 = manager.GetPool<PooledType>();

            Assert.NotNull(pool2);
            Assert.NotNull(pool3);
            Assert.AreEqual(pool, pool2);
            Assert.AreEqual(pool2, pool3);
        }

        [Test]
        public void PoolType()
        {
            var manager = new ObjectPoolManager();
            var pool = manager.GetPool<PooledType>();

            Assert.NotNull(pool);
            Assert.NotNull(pool.Get().GetType() == typeof(PooledType));
        }

        [Test]
        public void GetPrefabPools()
        {
            var manager = new ObjectPoolManager();
            PooledMono prefab = GameObject.Instantiate(new GameObject()).AddComponent<PooledMono>();
            var pool = manager.GetPool(prefab);

            Assert.NotNull(pool);

            var pool2 = manager.GetPool(prefab);
            var pool3 = manager.GetPool(prefab);

            Assert.NotNull(pool2);
            Assert.NotNull(pool3);
            Assert.AreEqual(pool, pool2);
            Assert.AreEqual(pool2, pool3);
        }

    }
}