using Assets.Nep.UI;
using Nap.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nap.UI
{
    /// <summary>
    /// Entry point of the UI framework. Keeps track of UI layers and UI controllers.
    /// Provides methods to change the state of the UI.
    /// </summary>
    public class UIFrame : MonoBehaviour
    {
        class UIControllerEntry
        {
            public UIControllerBase Controller { get; }
            public UILayer Layer { get; }
            public UIControllerEntry(UIControllerBase controller, UILayer parentLayer)
            {
                Controller = controller;
                Layer = parentLayer;
            }
        }

        private Dictionary<string, UILayer> _registeredUILayers = new Dictionary<string, UILayer>();
        private Dictionary<string, UIControllerEntry> _registeredControllers = new Dictionary<string, UIControllerEntry>();

        #region MonoBehaviour
        private void Awake()
        {
        }

        private void OnEnable()
        {
            GlobalEvents.Get<SignalUIOpenScreen>().Event += OnRequestOpen;
            GlobalEvents.Get<SignalUICloseScreen>().Event += OnRequestClose;
        }

        private void OnDisable()
        {
            GlobalEvents.Get<SignalUIOpenScreen>().Event -= OnRequestOpen;
            GlobalEvents.Get<SignalUICloseScreen>().Event -= OnRequestClose;
        }

        #endregion

        public void Init()
        {
            UILayer[] uiLayers = GetComponentsInChildren<UILayer>();
            foreach (var uiLayer in uiLayers)
            {
                RegisterUILayer(uiLayer.LayerId, uiLayer);
            }
        }

        public void OnRequestOpen(UIChangeScreenEvtArgs args)
        {
            Open(args.ScreenId, args.ShouldAnimate);
        }

        public void Open(string screenId, bool animate = true)
        {
            if (_registeredControllers.TryGetValue(screenId, out UIControllerEntry entry))
            {
                entry.Layer.ShowScreen(entry.Controller, animate);
            }
        }

        public void OnRequestClose(UIChangeScreenEvtArgs args)
        {
            Close(args.ScreenId, args.ShouldAnimate);
        }

        public void Close(string screenId, bool animate = true)
        {
            if (_registeredControllers.TryGetValue(screenId, out UIControllerEntry entry))
            {
                entry.Layer.HideScreen(entry.Controller, animate);
            }
        }

        private void RegisterUILayer(string layerId, UILayer uiLayer)
        {
            _registeredUILayers.Add(layerId, uiLayer);
        }

        public UILayer GetUILayer(string layerId)
        {
            if (_registeredUILayers.TryGetValue(layerId, out UILayer uiLayer))
            {
                return uiLayer;
            }
            else
            {
                return null;
            }
        }

        public void RegisterScreen(string screenId, UIControllerBase controller, UILayer layer)
        {
            _registeredControllers.Add(screenId, new UIControllerEntry(controller, layer));
        }

        public virtual void ReparentScreen(UIControllerBase controller, Transform screenTr)
        {
            screenTr.SetParent(transform, false);
        }
    }
}