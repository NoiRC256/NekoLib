using System.Collections.Generic;
using UnityEngine;
using NekoLib.Events;

namespace NekoLib.UI
{
    /// <summary>
    /// Entry point of the UI framework. Keeps track of UI layers and UI controllers.
    /// Provides methods to change the state of the UI.
    /// </summary>
    public class UIFrame : MonoSingleton<UIFrame>
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

        private Dictionary<int, UILayer> _registeredUILayers = new Dictionary<int, UILayer>();
        private Dictionary<string, UIControllerEntry> _registeredControllers = new Dictionary<string, UIControllerEntry>();

        #region MonoBehaviour
        private void OnEnable()
        {
            GlobalEvents.Get<UISignalOpenScreen>().Event += OnRequestOpen;
            GlobalEvents.Get<UISignalCloseScreen>().Event += OnRequestClose;
        }

        private void OnDisable()
        {
            GlobalEvents.Get<UISignalOpenScreen>().Event -= OnRequestOpen;
            GlobalEvents.Get<UISignalCloseScreen>().Event -= OnRequestClose;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        #endregion

        public override void Init()
        {
            UILayer[] uiLayers = GetComponentsInChildren<UILayer>();
            for (int i = 0; i < uiLayers.Length; i++)
            {
                var uiLayer = uiLayers[i];
                uiLayer.LayerId = i;
                RegisterUILayer(uiLayer.LayerId, uiLayer);
            }
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnRequestOpen(UISignalScreenEvtArgs args)
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

        public void OnRequestClose(UISignalScreenEvtArgs args)
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

        private void RegisterUILayer(int layerId, UILayer uiLayer)
        {
            _registeredUILayers.Add(layerId, uiLayer);
        }

        public UILayer GetUILayer(int layerId)
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