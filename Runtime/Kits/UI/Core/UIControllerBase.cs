using System;
using UnityEngine;
using NekoLib.Events;

namespace NekoLib.UI
{
    /// <summary>
    /// Base UI controller. Can be extended to implement business logic.
    /// </summary>
    public class UIControllerBase : MonoBehaviour
    {
        [field: SerializeField] public string ScreenId { get; set; }
        [field: SerializeField] public int LayerId { get; set; }
        [field: SerializeField] public TransitionBase AnimShow { get; set; }
        [field: SerializeField] public TransitionBase AnimHide { get; set; }

        public UIControllerBase ParentController { get; set; }

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
            GlobalEvents.Get<UISignalOpenScreen>().Invoke(new UISignalScreenEvtArgs(id));
        }

        public virtual void SignalCloseScreen(string id)
        {
            GlobalEvents.Get<UISignalCloseScreen>().Invoke(new UISignalScreenEvtArgs(id));
        }
    }
}