namespace Nep.UI
{
    public interface IWindowContext : IUIControllerContext
    {
        WindowPriority WindowPriority { get; set; }
        bool HideOnForegroundLost { get; set; }
        bool IsPopup { get; set; }
        bool SuppressPrefabContext { get; set; }
    }
}