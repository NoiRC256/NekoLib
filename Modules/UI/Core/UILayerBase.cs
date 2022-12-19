using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

namespace Nep.UI
{
    /// <summary>
    /// Base class for UI layer. A UI layer maintains screens registered to it.
    /// </summary>
    /// <typeparam name="TController">Type of UI screen controller interface</typeparam>
    public abstract class UILayerBase<TController> : MonoBehaviour where TController : IController
    {
        protected Dictionary<string, TController> _controllers;

        public virtual void Init()
        {
            _controllers = new Dictionary<string, TController>();
        }

        public virtual void ReparentScreen(IController controller, Transform screenTr)
        {
            screenTr.SetParent(transform, false);
        }

        #region Register
        protected virtual void InternalRegisterScreen(string id, TController controller)
        {
            controller.ScreenId = id;
            _controllers.Add(id, controller);
            controller.ScreenDestroyed += OnScreenDestroyed;
        }

        protected virtual void InternalUnregisterScreen(string id, TController controller)
        {
            controller.ScreenDestroyed -= OnScreenDestroyed;
            _controllers.Remove(id);
        }

        public void RegisterScreen(string id, TController controller)
        {
            if (!_controllers.ContainsKey(id)) InternalRegisterScreen(id, controller);
        }

        public void UnregisterScreen(string id, TController controller)
        {
            if (_controllers.ContainsKey(id)) InternalUnregisterScreen(id, controller);

        }

        private void OnScreenDestroyed(IController controller)
        {
            if (!string.IsNullOrEmpty(controller.ScreenId) && _controllers.ContainsKey(controller.ScreenId))
            {
                UnregisterScreen(controller.ScreenId, (TController)controller);
            }
        }
        #endregion

        #region Show and Hide
        public abstract void ShowScreen(TController controller);

        public abstract void ShowScreen<TModel>(TController controller, TModel properties) where TModel : IModel;

        public abstract void HideScreen(TController controller);

        public virtual void HideAll(bool shouldAnimateWhenHiding = true)
        {
            foreach (var screenEntry in _controllers)
            {
                screenEntry.Value.Hide(shouldAnimateWhenHiding);
            }
        }

        public void ShowScreenById(string id)
        {
            if (_controllers.TryGetValue(id, out TController controller)) ShowScreen(controller);
        }

        public void ShowScreenById<TProperties>(string id, TProperties properties) where TProperties : IModel
        {
            if (_controllers.TryGetValue(id, out TController controller)) ShowScreen(controller, properties);
        }

        public void HideScreenById(string id)
        {
            if (_controllers.TryGetValue(id, out TController controller)) HideScreen(controller);
        }
        #endregion
    }
}