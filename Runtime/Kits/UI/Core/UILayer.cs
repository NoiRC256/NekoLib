using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

namespace NekoLib.UI
{
    /// <summary>
    /// Base class for UI layer. A UI layer maintains screens registered to it.
    /// </summary>
    /// <typeparam name="TController">Type of UI screen controller interface</typeparam>
    public class UILayer : MonoBehaviour
    {
        [field:SerializeField] public Canvas RootCanvas { get; private set; }
        public int LayerId { get; set; }
        public Camera UICamera => RootCanvas.worldCamera;
        public UIPageControllerBase CurrentPage { get; set; }

        protected Dictionary<string, UIControllerBase> _controllers = new Dictionary<string, UIControllerBase>();
        protected Stack<UIPageControllerBase> _pageStack = new Stack<UIPageControllerBase>();

        public UILayer(int layerId, Canvas rootCanvas)
        {
            LayerId = layerId;
            RootCanvas = rootCanvas;
        }

        public virtual void Init()
        {
        }

        #region Register
        public void RegisterScreen(string id, UIControllerBase controller)
        {
            if (!_controllers.ContainsKey(id)) InternalRegisterScreen(id, controller);
        }

        protected virtual void InternalRegisterScreen(string id, UIControllerBase controller)
        {
            controller.ScreenId = id;
            _controllers.Add(id, controller);
        }

        public void UnregisterScreen(string id, UIControllerBase controller)
        {
            if (_controllers.ContainsKey(id)) InternalUnregisterScreen(id, controller);

        }

        protected virtual void InternalUnregisterScreen(string id, UIControllerBase controller)
        {
            _controllers.Remove(id);
        }

        private void OnScreenDestroyed(UIControllerBase controller)
        {
            if (!string.IsNullOrEmpty(controller.ScreenId) && _controllers.ContainsKey(controller.ScreenId))
            {
                UnregisterScreen(controller.ScreenId, controller);
            }
        }
        #endregion

        #region Show
        public void ShowScreen(string id)
        {
            if (_controllers.TryGetValue(id, out UIControllerBase controller))
            {
                ShowScreen(controller);
            }
        }

        public void ShowScreen<TContext>(string id, TContext context) where TContext : UIControllerContextBase
        {
            if (_controllers.TryGetValue(id, out UIControllerBase controller))
            {
                ShowScreen(controller, context);
            }
        }

        public virtual void ShowScreen(UIControllerBase controller, bool animate = true)
        {
            controller.UIShow(animate: animate);
        }

        public virtual void ShowScreen<TContext>(UIControllerBase controller, TContext context, bool animate = true)
            where TContext : UIControllerContextBase
        {
            controller.UIShow(context, animate);
        }
        #endregion

        #region Hide
        public void HideScreenById(string id)
        {
            if (_controllers.TryGetValue(id, out UIControllerBase controller))
            {
                HideScreen(controller);
            }
        }

        public virtual void HideScreen(UIControllerBase controller, bool animate = true)
        {
            controller.UIHide(animate);
        }

        public virtual void HideAll(bool animate = true)
        {
            foreach (var screenEntry in _controllers)
            {
                screenEntry.Value.UIHide(animate);
            }
        }
        #endregion
    }
}