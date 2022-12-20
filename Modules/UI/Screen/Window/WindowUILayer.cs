using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nep.UI
{
    public class WindowUILayer : UILayerBase<IWindowController>
    {
        public IWindowController CurrentWindow { get; private set; }

        private Queue<WindowHistoryEntry> _windowQueue;
        private Stack<WindowHistoryEntry> _windowHistory;
        private HashSet<IUIController> _transitioningScreens;

        public event Action RequestScreenBlock;
        public event Action RequestScreenUnblock;

        private bool IsScreenTransitioning => _transitioningScreens.Count != 0;

        public override void Init()
        {
            base.Init();
            _controllers = new Dictionary<string, IWindowController>();
            _windowQueue = new Queue<WindowHistoryEntry>();
            _windowHistory = new Stack<WindowHistoryEntry>();
            _transitioningScreens = new HashSet<IUIController>();
        }

        public override void ReparentScreen(IUIController controller, Transform screenTr)
        {
            IWindowController window = controller as IWindowController;

            if (window == null)
            {
                Debug.LogError("[WindowUILayer] Screen " + screenTr.name + " is not a Window!");
            }
            else
            {
                if (window.IsPopup)
                {
                    _priorityParaLayer.AddScreen(screenTr);
                    return;
                }
            }

            base.ReparentScreen(controller, screenTr);
        }

        #region Register
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

        private void OnCloseRequestedByWindow(IUIController screen)
        {
            HideScreen(screen as IWindowController);
        }

        private void OnTransitionInFinished(IUIController screen)
        {
            RemoveTransition(screen);
        }

        private void OnTransitionOutFinished(IUIController screen)
        {
            RemoveTransition(screen);
            var window = screen as IWindowController;
            if (window.IsPopup)
            {
                _priorityParaLayer.RefreshDarken();
            }
        }
        #endregion

        #region Show and Hide
        public override void ShowScreen(IWindowController controller)
        {
            throw new System.NotImplementedException();
        }

        public override void ShowScreen<TProperties>(IWindowController controller, TProperties properties)
        {
            IWindowContext windowProperties = (IWindowContext)properties;
            if (ShouldEnqueue(controller, windowProperties))
            {
                EnqueueWindow(controller, properties);
            }
            else
            {
                DoShow(controller, windowProperties);
            }
        }

        public override void HideScreen(IWindowController screen)
        {
            if (screen == CurrentWindow)
            {
                _windowHistory.Pop();
                AddTransition(screen);
                screen.Hide();

                CurrentWindow = null;

                if (_windowQueue.Count > 0) { } //Show next window in queue.
                else if (_windowHistory.Count > 0) { }//Otherwise, show previous window in history.
            }
        }

        public override void HideAll(bool shouldAnimate = true)
        {
            base.HideAll(shouldAnimate);
            CurrentWindow = null;
            _priorityParaLayer.RefreshDarken();
            _windowHistory.Clear();
        }

        private void EnqueueWindow<TProperties>(IWindowController controller, TProperties properties)
        {
            _windowQueue.Enqueue(new WindowHistoryEntry(controller, (IWindowContext)properties));
        }

        private bool ShouldEnqueue(IWindowController controller, IWindowContext windowProperties)
        {
            if (CurrentWindow == null && _windowQueue.Count == 0)
            {
                return false;
            }
            if (windowProperties != null && windowProperties.SuppressPrefabContext)
            {
                return windowProperties.WindowPriority != WindowPriority.ForceForeground;
            }
            if (controller.WindowPriority != WindowPriority.ForceForeground)
            {
                return true;
            }
            return false;
        }

        private void ShowPreviousInHistory()
        {
            if (_windowHistory.Count > 0)
            {
                WindowHistoryEntry window = _windowHistory.Pop();
                DoShow(window);
            }
        }

        private void ShowNextInQueue()
        {
            if (_windowQueue.Count > 0)
            {
                WindowHistoryEntry window = _windowQueue.Dequeue();
                DoShow(window);
            }
        }

        private void DoShow(IWindowController controller, IWindowContext windowProperties)
        {
            DoShow(new WindowHistoryEntry(controller, windowProperties));
        }

        private void DoShow(WindowHistoryEntry windowHistoryEntry)
        {
            if (CurrentWindow == windowHistoryEntry.Controller)
            {
                Debug.LogWarning(
                    string.Format(
                        "[WindowUILayer] The requested WindowId ({0}) is already open! This will add a duplicate to the " +
                        "history and might cause inconsistent behaviour. It is recommended that if you need to open the same" +
                        "screen multiple times (eg: when implementing a warning message pop-up), it closes itself upon the player input" +
                        "that triggers the continuation of the flow."
                        , CurrentWindow.ScreenId));
            }
            else if (CurrentWindow != null
                     && CurrentWindow.HideOnForegroundLost
                     && !windowHistoryEntry.Controller.IsPopup)
            {
                CurrentWindow.Hide();
            }

            _windowHistory.Push(windowHistoryEntry);
            AddTransition(windowHistoryEntry.Controller);

            if (windowHistoryEntry.Controller.IsPopup)
            {
                _priorityParaLayer.DarkenBG();
            }

            windowHistoryEntry.Show();

            CurrentWindow = windowHistoryEntry.Controller;
        }
        #endregion

        #region Transition
        private void AddTransition(IUIController screen)
        {
            _transitioningScreens.Add(screen);
            if (RequestScreenBlock != null)
            {
                RequestScreenBlock();
            }
        }

        private void RemoveTransition(IUIController screen)
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
        #endregion
    }
}