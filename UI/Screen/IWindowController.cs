namespace NekoSystems.UI
{
    public interface IWindowController : IScreenController
    {
        WindowPriority WindowPriority { get; }
        bool HideOnForegroundLost { get; }
        bool IsPopup { get; }
    }
}