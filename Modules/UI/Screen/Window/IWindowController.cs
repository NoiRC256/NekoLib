namespace Nep.UI
{
    public interface IWindowController : IController
    {
        WindowPriority WindowPriority { get; }
        bool HideOnForegroundLost { get; }
        bool IsPopup { get; }
    }
}