using UnityEngine;

namespace NekoLab.NekoPhysics
{
    public static partial class PhysicsUtil
    {
        public static bool LayerMaskContains(LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | 1 << layer);
        }
    }
}