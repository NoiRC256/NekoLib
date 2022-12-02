using System;
using UnityEngine;

namespace NekoSystems.Pool
{
    /// <summary>
    /// Prefab pool with default delegates for pooling gameobjects.
    /// </summary>
    public class AutoGameObjectPool : AutoPool<GameObject>, IObjectPool<GameObject>
    {
        private GameObject _obj;

        protected override GameObject DefaultCreateFunc()
        {
            return GameObject.Instantiate(_obj);
        }

        protected override void DefaultOnRelease(GameObject obj)
        {
            obj.SetActive(false);
        }

        protected override void DefaultOnDestroy(GameObject obj)
        {
            GameObject.Destroy(obj);
        }
    }
}