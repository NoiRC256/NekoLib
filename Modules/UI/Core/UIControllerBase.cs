using System;
using UnityEngine;
using Nap.Events;
using Assets.Nep.UI;

namespace Nap.UI
{
    /// <summary>
    /// Base UI controller. Can be extended to implement business logic.
    /// </summary>
    public class UIControllerBase : MonoBehaviour
    {
        [field: SerializeField] public string ScreenId { get; set; }
        [field: SerializeField] public string LayerId { get; set; }
        [field: SerializeField] public TransitionBase AnimShow { get; set; }
        [field: SerializeField] public TransitionBase AnimHide { get; set; }

        public Action<UIControllerBase> ScreenDestroyed { get; set; }

        #region Show
        public virtual void UIShow(UIControllerContextBase context = null, bool animate = true)
        {
            if (context != null) SetContext(context);
            Show();
            OnUIShow();
            if (animate)
            {
                AnimShow.PlayAnimation(this.transform);
            }
        }

        public virtual void SetContext(UIControllerContextBase context)
        {

        }

        public virtual void Show()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void OnUIShow()
        {

        }
        #endregion

        #region Hide
        public virtual void UIHide(bool animate = true)
        {
            if (animate)
            {
                AnimShow.PlayAnimation(this.transform, Hide);
            }
            else
            {
                Hide();
            }
        }

        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
        }
        #endregion

        public virtual void SignalOpenScreen(string id)
        {
            GlobalEvents.Get<SignalUIOpenScreen>().Invoke(this, new UIChangeScreenEvtArgs(id));
        }

        public virtual void SignalCloseScreen(string id)
        {
            GlobalEvents.Get<SignalUICloseScreen>().Invoke(this, new UIChangeScreenEvtArgs(id));
        }
    }
}