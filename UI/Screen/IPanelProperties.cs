namespace NekoSystems.UI
{
    public interface IPanelProperties : IScreenProperties
    {
        PanelPriority Priority { get; set; }
    }
}