using System;
using UnityEngine;

namespace Nap.Pool
{
    /// <summary>
    /// Prefab pool with default delegates for pooling gameobjects referenced by components.
    /// </summary>
    public class AutoComponentPool : AutoPool<Component>, IObjectPool<Component>
    {
        private Component _obj;

        protected override Component DefaultCreateFunc()
        {
            return GameObject.Instantiate(_obj);
        }

        protected override void DefaultOnRelease(Component obj)
        {
            obj.gameObject.SetActive(false);
        }

        protected override void DefaultOnDestroy(Component obj)
        {
            GameObject.Destroy(obj);
        }
    }
}