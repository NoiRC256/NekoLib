using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

namespace Nep.UI
{
    /// <summary>
    /// Base class for UI layer. A UI layer maintains screens registered to it.
    /// </summary>
    /// <typeparam name="TScreen"></typeparam>
    public abstract class UILayerBase<TScreen> : MonoBehaviour where TScreen : IScreenController
    {
        protected Dictionary<string, TScreen> _screens;

        public virtual void Init()
        {
            _screens = new Dictionary<string, TScreen>();
        }

        public virtual void ReparentScreen(IScreenController controller, Transform screenTr)
        {
            screenTr.SetParent(transform, false);
        }

        #region Register
        protected virtual void InternalRegisterScreen(string id, TScreen controller)
        {
            controller.ScreenId = id;
            _screens.Add(id, controller);
            controller.ScreenDestroyed += OnScreenDestroyed;
        }

        protected virtual void InternalUnregisterScreen(string id, TScreen controller)
        {
            controller.ScreenDestroyed -= OnScreenDestroyed;
            _screens.Remove(id);
        }

        public void RegisterScreen(string id, TScreen controller)
        {
            if (!_screens.ContainsKey(id)) InternalRegisterScreen(id, controller);
        }

        public void UnregisterScreen(string id, TScreen controller)
        {
            if (_screens.ContainsKey(id)) InternalUnregisterScreen(id, controller);

        }

        private void OnScreenDestroyed(IScreenController screen)
        {
            if (!string.IsNullOrEmpty(screen.ScreenId) && _screens.ContainsKey(screen.ScreenId))
            {
                UnregisterScreen(screen.ScreenId, (TScreen)screen);
            }
        }
        #endregion

        #region Show and Hide
        public abstract void ShowScreen(TScreen screen);

        public abstract void ShowScreen<TProperties>(TScreen screen, TProperties properties) where TProperties : IScreenProperties;

        public abstract void HideScreen(TScreen screen);

        public virtual void HideAll(bool shouldAnimateWhenHiding = true)
        {
            foreach (var screenEntry in _screens)
            {
                screenEntry.Value.Hide(shouldAnimateWhenHiding);
            }
        }

        public void ShowScreenById(string id)
        {
            if (_screens.TryGetValue(id, out TScreen screen)) ShowScreen(screen);
        }

        public void ShowScreenById<TProperties>(string id, TProperties properties) where TProperties : IScreenProperties
        {
            if (_screens.TryGetValue(id, out TScreen screen)) ShowScreen(screen, properties);
        }

        public void HideScreenById(string id)
        {
            if (_screens.TryGetValue(id, out TScreen screen)) HideScreen(screen);
        }
        #endregion
    }
}