namespace Nep.UI
{
    public abstract class WindowControllerBase : WindowControllerBase<WindowContextBase> { }

    public abstract class WindowControllerBase<TContext> : UIControllerBase<WindowContextBase>, IWindowController
        where TContext : IWindowContext
    {
        public WindowPriority WindowPriority => Context.WindowPriority;
        public bool HideOnForegroundLost => Context.HideOnForegroundLost;
        public bool IsPopup => Context.IsPopup;

        public virtual void UIClose()
        {
            CloseRequest(this);
        }

        protected sealed override void SetContext(WindowContextBase windowContext)
        {
            if(windowContext != null)
            {
                if (!windowContext.SuppressPrefabContext)
                {
                    windowContext.WindowPriority = Context.WindowPriority;
                    windowContext.HideOnForegroundLost = Context.HideOnForegroundLost;
                    windowContext.IsPopup = Context.IsPopup;
                }
                Context = windowContext;
            }
        }

        protected override void ChangeHierarchy()
        {
            transform.SetAsLastSibling();
        }
    }
}