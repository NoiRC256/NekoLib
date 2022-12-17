using System;
using UnityEngine;

namespace Nep.UI
{
    public class ScreenControllerBase<TProperties> : MonoBehaviour, IScreenController where TProperties : IScreenProperties
    {
        public string ScreenId { get; set; }
        public bool IsVisible { get; set; }
        public Action<IScreenController> TransitionInFinished { get; set; }
        public Action<IScreenController> TransitionOutFinished { get; set; }
        public Action<IScreenController> CloseRequest { get; set; }
        public Action<IScreenController> ScreenDestroyed { get; set; }
        public TransitionBase AnimIn { get => _animIn; set => _animIn = value; }
        public TransitionBase AnimOut { get => _animOut; set => _animOut = value; }
        protected TProperties Properties { get => _properties; set => _properties = value; }

        private TransitionBase _animIn;
        private TransitionBase _animOut;
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
            if (properties != null)
            {
                if (properties is TProperties) SetProperties((TProperties)properties);
            }
            HierarchyFixOnShow();
            OnPropertiesSet();
            if (!this.gameObject.activeSelf) DoTransition(_animIn, OnAnimInComplete, true);
            else TransitionInFinished?.Invoke(this);
        }

        public void Hide(bool animate = true)
        {
            DoTransition(animate ? _animOut : null, OnAnimOutComplete, false);
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

        private void OnAnimInComplete()
        {
            IsVisible = true;
            TransitionInFinished?.Invoke(this);
        }

        private void OnAnimOutComplete()
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