using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekoSystems.UI
{
    public class WindowUILayer : UILayerBase<IWindowController>
    {
        public IWindowController CurrentWindow { get; private set; }

        private Queue<WindowHistoryEntry> _windowQueue;
        private Stack<WindowHistoryEntry> _windowHistory;
        private HashSet<IScreenController> _transitioningScreens;

        public event Action RequestScreenBlock;
        public event Action RequestScreenUnblock;

        private bool IsScreenTransitioning => _transitioningScreens.Count != 0;

        public override void ShowScreen(IWindowController screen)
        {
            throw new System.NotImplementedException();
        }

        public override void ShowScreen<TProperties>(IWindowController screen, TProperties properties)
        {
            throw new System.NotImplementedException();
        }

        public override void HideScreen(IWindowController screen)
        {
            if (screen == CurrentWindow)
            {
                _windowHistory.Pop();
                AddTransition(screen);
                screen.Hide();

                CurrentWindow = null;

                if (_windowQueue.Count > 0) { } //Show next window.
                else if (_windowHistory.Count > 0) { }//Show previous window in history.
            }
        }

        public override void Init()
        {
            base.Init();
            _screens = new Dictionary<string, IWindowController>();
            _windowQueue = new Queue<WindowHistoryEntry>();
            _windowHistory = new Stack<WindowHistoryEntry>();
            _transitioningScreens = new HashSet<IScreenController>();
        }

        protected override void InternalRegisterScreen(string id, IWindowController controller)
        {
            base.InternalRegisterScreen(id, controller);
            controller.TransitionInFinished += OnTransitionInFinished;
            controller.TransitionOutFinished += OnTransitionOutFinished;
            controller.CloseRequest += OnCloseRequestedByWindow;
        }

        protected override void InternalUnregisterScreen(string id, IWindowController controller)
        {
            base.InternalUnregisterScreen(id, controller);
            controller.TransitionInFinished -= OnTransitionInFinished;
            controller.TransitionOutFinished -= OnTransitionOutFinished;
            controller.CloseRequest -= OnCloseRequestedByWindow;
        }

        private void OnTransitionInFinished(IScreenController screen)
        {
            RemoveTransition(screen);
        }

        private void OnTransitionOutFinished(IScreenController screen)
        {
            RemoveTransition(screen);
            var window = screen as IWindowController;
            if (window.IsPopup)
            {
                //priorityParaLayer.RefreshDarken();
            }
        }

        private void OnCloseRequestedByWindow(IScreenController screen)
        {
            HideScreen(screen as IWindowController);
        }

        private void AddTransition(IScreenController screen)
        {
            _transitioningScreens.Add(screen);
            if (RequestScreenBlock != null)
            {
                RequestScreenBlock();
            }
        }

        private void RemoveTransition(IScreenController screen)
        {
            _transitioningScreens.Remove(screen);
            if (!IsScreenTransitioning)
            {
                if (RequestScreenUnblock != null)
                {
                    RequestScreenUnblock();
                }
            }
        }
    }
}