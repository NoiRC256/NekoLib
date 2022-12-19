namespace Nep.UI
{
    public interface IWindowModel : IModel
    {
        WindowPriority WindowPriority { get; set; }
        bool HideOnForegroundLost { get; set; }
        bool IsPopup { get; set; }
        bool SuppressPrefabProperties { get; set; }
    }
}