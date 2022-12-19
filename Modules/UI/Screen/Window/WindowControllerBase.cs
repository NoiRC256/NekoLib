namespace Nep.UI
{
    public abstract class WindowControllerBase : WindowControllerBase<WindowModel> { }

    public abstract class WindowControllerBase<TModel> : ControllerBase<WindowModel>, IWindowController
        where TModel : IWindowModel
    {
        public WindowPriority WindowPriority => Properties.WindowPriority;

        public bool HideOnForegroundLost => Properties.HideOnForegroundLost;

        public bool IsPopup => Properties.IsPopup;

        public virtual void UIClose()
        {
            CloseRequest(this);
        }

        protected sealed override void SetProperties(WindowModel windowModel)
        {
            if(windowModel != null)
            {
                if (!windowModel.SuppressPrefabProperties)
                {
                    windowModel.WindowPriority = Properties.WindowPriority;
                    windowModel.HideOnForegroundLost = Properties.HideOnForegroundLost;
                    windowModel.IsPopup = Properties.IsPopup;
                }
                Properties = windowModel;
            }
        }

        protected override void HierarchyFixOnShow()
        {
            transform.SetAsLastSibling();
        }
    }
}