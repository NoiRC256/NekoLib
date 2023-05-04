using UnityEngine;

namespace NekoLib.Movement
{
    public class GroundSensor
    {
        public float GroundThresholdDistance { get; set; }
        public LayerMask LayerMask { get; set; }

        /// <summary>
        /// Probe ground from origin downwards for a specified range.
        /// Uses the configured layer mask.
        /// </summary>
        /// <param name="groundInfo"></param>
        /// <param name="range"></param>
        /// <param name="thickness"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public bool ProbeGround(out GroundInfo groundInfo, float range, float thickness, Vector3 origin, bool useRealGroundNormal = false)
        {
            return ProbeGround(out groundInfo, range, thickness, origin, LayerMask, useRealGroundNormal);
        }

        /// <summary>
        /// Probe ground from origin downwards for a specified range.
        /// </summary>
        /// <param name="groundInfo"></param>
        /// <param name="range"></param>
        /// <param name="thickness"></param>
        /// <param name="origin"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public bool ProbeGround(out GroundInfo groundInfo, float range, float thickness, Vector3 origin, LayerMask layerMask,
            bool useRealGroundNormal = false)
        {
            groundInfo = GroundInfo.Empty;
            bool isGroundInRange = false;

            bool hasHit = false;
            RaycastHit hitInfo;
            if (thickness <= 0f) hasHit = Physics.Raycast(origin, Vector3.down, out hitInfo,
                    maxDistance: range, layerMask: layerMask);
            else hasHit = Physics.SphereCast(origin, thickness / 2f, Vector3.down, out hitInfo,
                    maxDistance: range, layerMask: layerMask);

            if (hasHit)
            {
                groundInfo.Distance = origin.y - hitInfo.point.y;
                if (isWithinGround(groundInfo.Distance))
                {
                    isGroundInRange = true;
                    groundInfo.Normal = hitInfo.normal;
                    groundInfo.Point = hitInfo.point;
                    groundInfo.Collider = hitInfo.collider;

                    if (useRealGroundNormal && thickness > 0f)
                    {
                        Vector3 tmpOrigin = hitInfo.point + new Vector3(0f, 0.01f, 0f);
                        RaycastHit realNormalHitInfo;
                        if (hitInfo.collider.Raycast(new Ray(tmpOrigin, Vector3.down), out realNormalHitInfo, maxDistance: 0.1f))
                        {
                            groundInfo.Normal = realNormalHitInfo.normal;
                        }
                    }
                }
            }

            groundInfo.IsOnGround = isGroundInRange;
            return isGroundInRange;
        }

        /// <summary>
        /// Returns true if the provided ground distance is within ground distance threshold.
        /// </summary>
        /// <param name="groundDistance"></param>
        /// <returns></returns>
        public bool isWithinGround(float groundDistance)
        {
            return groundDistance <= GroundThresholdDistance;
        }
    }
}