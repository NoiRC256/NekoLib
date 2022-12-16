using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nep.UI
{
    /// <summary>
    /// A UI controller defines logic between the UI view and UI model.
    /// </summary>
    public abstract class UIControllerBase<TView, TModel, TContext> : MonoBehaviour, IUIController 
        where TView : UIViewBase where TModel : UIModelBase where TContext : UIControllerContextBase
    {
        public string ControllerId { get; set; }
        public TView View { get; set; }
        public TModel Model { get; set; }
        public Action<IUIController> ControllerDestroyed { get; set; }

        public void Show(UIControllerContextBase context = null)
        {
            OnShow(context);
        }

        protected virtual void OnShow(UIControllerContextBase context = null)
        {

        }

        public void Hide(bool shouldAnimate = true)
        {
            OnHide(shouldAnimate);
        }

        protected virtual void OnHide(bool shouldAnimate = true)
        {

        }
    }
}