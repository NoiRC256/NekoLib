using System;
using UnityEngine;

namespace NekoLib.Pool
{
    /// <summary>
    /// Prefab pool with default delegates.
    /// <para>Pools instances of a prefb referenced by <see cref="GameObject"/>.</para>
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