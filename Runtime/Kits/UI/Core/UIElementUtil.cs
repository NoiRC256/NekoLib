using UnityEngine;
using UnityEngine.UI;

namespace NekoLib.UI
{
    /// <summary>
    /// Provides UI element utility methods.
    /// </summary>
    public class UIElementUtil
    {
        /// <summary>
        /// Calculate screenspace position of specified worldspace transform.
        /// </summary>
        /// <param name="followerRect">The follower UI element rect</param>
        /// <param name="followTr">The worldspace transform to follow</param>
        /// <param name="viewCam">The view camera used for viewport projection calculation</param>
        /// <param name="canvasScaler">The canvas scaler used for adaptive scale calculation</param>
        /// <returns></returns>
        public static Vector2 CalcScreenPos(Rect followerRect, Transform followTr, Camera viewCam, CanvasScaler canvasScaler)
        {
            Vector3 relPos = viewCam.transform.InverseTransformPoint(followTr.position);
            relPos.z = Mathf.Max(relPos.z, 1f);
            Vector3 viewportPos = viewCam.WorldToViewportPoint(viewCam.transform.TransformPoint(relPos));
            return new Vector2(
                viewportPos.x * canvasScaler.referenceResolution.x - followerRect.size.x / 2f,
                viewportPos.y * canvasScaler.referenceResolution.y - followerRect.size.y / 2f);
        }

        /// <summary>
        /// Clamp screenspace position based on specified rect transform.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="screenPos"></param>
        /// <param name="rootRectTr"></param>
        /// <returns></returns>
        public static Vector2 ClampScreenPos(Rect rect, Vector2 screenPos, RectTransform rootRectTr)
        {
            return new Vector2(
                Mathf.Clamp(screenPos.x, 0f, rootRectTr.sizeDelta.x - rect.size.x),
                Mathf.Clamp(screenPos.y, 0f, rootRectTr.sizeDelta.y - rect.size.y));
        }
    }
}