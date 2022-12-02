using System;
using UnityEngine;

namespace NekoSystems.UI
{
    public class ScreenControllerBase<TProperties> : MonoBehaviour, IScreenController where TProperties : IScreenProperties
    {
        public string ScreenId { get; set; }
        public bool IsVisible { get; set; }
        public Action<IScreenController> TransitionInFinished { get; set; }
        public Action<IScreenController> TransitionOutFinished { get; set; }
        public Action<IScreenController> CloseRequest { get; set; }
        public Action<IScreenController> ScreenDestroyed { get; set; }
        public TransitionBase TransitionIn { get => _transitionIn; set => _transitionIn = value; }
        public TransitionBase TransitionOut { get => _transitionOut; set => _transitionOut = value; }
        protected TProperties Properties { get => _properties; set => _properties = value; }

        private TransitionBase _transitionIn;
        private TransitionBase _transitionOut;
        private TProperties _properties;

        #region MonoBehaviour
        protected virtual void Awake()
        {
            AddListeners();
        }

        protected virtual void OnDestroy()
        {
            ScreenDestroyed?.Invoke(this);
            TransitionInFinished = null;
            TransitionOutFinished = null;
            CloseRequest = null;
            ScreenDestroyed = null;
            RemoveListeners();
        }
        #endregion

        #region Show and Hide
        public void Show(IScreenProperties properties = null)
        {
            if(properties != null)
            {
                if (properties is TProperties) SetProperties((TProperties)properties);
            }
            HierarchyFixOnShow();
            OnPropertiesSet();
            if (!this.gameObject.activeSelf) DoTransition(_transitionIn, OnTransitionInComplete, true);
            else TransitionInFinished?.Invoke(this);
        }

        public void Hide(bool animate = true)
        {
            DoTransition(animate ? _transitionOut : null, OnTransitionOutComplete, false);
            WhileHiding();
        }
        #endregion

        #region Transition
        private void DoTransition(TransitionBase transition, Action onComplete, bool isVisible)
        {
            if (transition != null)
            {
                this.gameObject.SetActive(isVisible);
                onComplete?.Invoke();
            }
            else
            {
                if (isVisible && !this.gameObject.activeSelf)
                {
                    this.gameObject.SetActive(true);
                }
                transition.Animate(this.transform, onComplete);
            }
        }

        private void OnTransitionInComplete()
        {
            IsVisible = true;
            TransitionInFinished?.Invoke(this);

        }

        private void OnTransitionOutComplete()
        {
            IsVisible = false;
            this.gameObject.SetActive(false);
            TransitionOutFinished?.Invoke(this);
        }
        #endregion

        protected virtual void SetProperties(TProperties properties)
        {
            _properties = properties;
        }

        protected virtual void OnPropertiesSet()
        {

        }

        protected virtual void HierarchyFixOnShow()
        {

        }

        protected virtual void WhileHiding()
        {

        }

        protected virtual void AddListeners()
        {

        }

        protected virtual void RemoveListeners()
        {

        }
    }
}