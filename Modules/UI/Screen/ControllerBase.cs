using System;
using UnityEngine;

namespace Nep.UI
{
    public class ControllerBase<TModel> : MonoBehaviour, IController where TModel : IModel
    {
        public string ScreenId { get; set; }
        public bool IsVisible { get; set; }
        public Action<IController> TransitionInFinished { get; set; }
        public Action<IController> TransitionOutFinished { get; set; }
        public Action<IController> CloseRequest { get; set; }
        public Action<IController> ScreenDestroyed { get; set; }
        public TransitionBase AnimIn { get => _animIn; set => _animIn = value; }
        public TransitionBase AnimOut { get => _animOut; set => _animOut = value; }
        protected TModel Properties { get => _model; set => _model = value; }

        private TransitionBase _animIn;
        private TransitionBase _animOut;
        private TModel _model;

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
        public void Show(IModel model = null)
        {
            if (model != null)
            {
                if (model is TModel) SetProperties((TModel)model);
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

        protected virtual void SetProperties(TModel model)
        {
            _model = model;
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