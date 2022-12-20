using System;
using UnityEngine;

namespace Nep.UI
{
    public class UIControllerBase<TContext> : MonoBehaviour, IUIController where TContext : IUIControllerContext
    {
        public string ScreenId { get; set; }
        public bool IsVisible { get; set; }
        protected TContext Context { get => _context; set => _context = value; }
        public TransitionBase AnimIn { get => _animIn; set => _animIn = value; }
        public TransitionBase AnimOut { get => _animOut; set => _animOut = value; }

        public Action<IUIController> TransitionInFinished { get; set; }
        public Action<IUIController> TransitionOutFinished { get; set; }
        public Action<IUIController> CloseRequest { get; set; }
        public Action<IUIController> ScreenDestroyed { get; set; }

        private TContext _context;
        private TransitionBase _animIn;
        private TransitionBase _animOut;

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
        public void Show(IUIControllerContext context = null)
        {
            if (context != null)
            {
                if (context is TContext) SetContext((TContext)context);
            }
            ChangeHierarchy();
            OnContextSet();
            if (!this.gameObject.activeSelf) DoTransition(_animIn, OnAnimInComplete, true);
            else TransitionInFinished?.Invoke(this);
        }

        protected virtual void SetContext(TContext context)
        {
            _context = context;
        }

        protected virtual void ChangeHierarchy()
        {

        }

        protected virtual void OnContextSet()
        {

        }

        public void Hide(bool animate = true)
        {
            DoTransition(animate ? _animOut : null, OnAnimOutComplete, false);
            WhileHiding();
        }

        protected virtual void WhileHiding()
        {

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
                transition.PlayAnimation(this.transform, onComplete);
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

        protected virtual void AddListeners()
        {

        }

        protected virtual void RemoveListeners()
        {

        }
    }
}