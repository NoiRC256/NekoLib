using UnityEngine;

namespace NekoLib.Pool
{
    /// <summary>
    /// Gameobject pool. Can be used to pool instances of a prefab.
    /// </summary>
    public class GameObjectPool : ObjectPoolBase<GameObject>
    {
        private GameObject _prefab;

        public GameObjectPool(GameObject prefab, int capacity = kDefaultCapacity) : base(capacity)
        {
            _prefab = prefab;
        }

        protected override GameObject Create()
        {
            return GameObject.Instantiate(_prefab);
        }

        protected override void Destroy(GameObject obj)
        {
            GameObject.Destroy(obj);
        }
    }
}
