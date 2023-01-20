using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace NekoLib.Pool.Test
{
    public class ObjectPoolTest
    {
        class PooledType
        {
            public void OnReturnToPool()
            {

            }

            public void Destroy()
            {

            }
        }

        class PooledMono : MonoBehaviour
        {
            public void OnReturnToPool()
            {

            }

            public void Destroy()
            {
                GameObject.Destroy(this);
            }
        }

        [Test]
        public void RegisterPool()
        {
            var manager = GameObject.Instantiate(new GameObject()).AddComponent<ObjectPoolManager>();

            Assert.IsNotNull(manager);

            Assert.IsFalse(manager.HasPool<PooledType>());

            manager.RegisterPool<PooledType>();

            Assert.IsTrue(manager.HasPool<PooledType>());
        }

        [Test]
        public void GetMultiplePools()
        {
            var manager = GameObject.Instantiate(new GameObject()).AddComponent<ObjectPoolManager>();
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
            var manager = GameObject.Instantiate(new GameObject()).AddComponent<ObjectPoolManager>();
            var pool = manager.GetPool<PooledType>();

            Assert.NotNull(pool);
            Assert.NotNull(pool.Get().GetType() == typeof(PooledType));
        }

        [Test]
        public void GetPrefabPools()
        {
            var manager = GameObject.Instantiate(new GameObject()).AddComponent<ObjectPoolManager>();
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