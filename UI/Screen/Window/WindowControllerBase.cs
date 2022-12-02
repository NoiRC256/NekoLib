namespace NekoSystems.UI
{
    public abstract class WindowControllerBase : WindowControllerBase<WindowProperties> { }

    public abstract class WindowControllerBase<TProperties> : ScreenControllerBase<WindowProperties>, IWindowController
        where TProperties : IWindowProperties
    {
        public WindowPriority WindowPriority => Properties.WindowPriority;

        public bool HideOnForegroundLost => Properties.HideOnForegroundLost;

        public bool IsPopup => Properties.IsPopup;

        public virtual void UIClose()
        {
            CloseRequest(this);
        }

        protected sealed override void SetProperties(WindowProperties properties)
        {
            if(properties != null)
            {
                if (!properties.SuppressPrefabProperties)
                {
                    properties.WindowPriority = Properties.WindowPriority;
                    properties.HideOnForegroundLost = Properties.HideOnForegroundLost;
                    properties.IsPopup = Properties.IsPopup;
                }
                Properties = properties;
            }
        }

        protected override void HierarchyFixOnShow()
        {
            transform.SetAsLastSibling();
        }
    }
}