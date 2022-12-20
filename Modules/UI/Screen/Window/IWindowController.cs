namespace Nep.UI
{
    public interface IWindowController : IUIController
    {
        WindowPriority WindowPriority { get; }
        bool HideOnForegroundLost { get; }
        bool IsPopup { get; }
    }
}