using UnityEngine;

namespace NekoLib.NekoPhysics
{
    public static partial class Physics3D
    {
        public static bool LayerMaskContains(LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | 1 << layer);
        }
    }
}