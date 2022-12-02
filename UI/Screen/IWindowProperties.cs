namespace NekoSystems.UI
{
    public interface IWindowProperties : IScreenProperties
    {
        WindowPriority WindowPriority { get; set; }
        bool HideOnForegroundLost { get; set; }
        bool IsPopup { get; set; }
        bool SuppressPrefabProperties { get; set; }
    }
}