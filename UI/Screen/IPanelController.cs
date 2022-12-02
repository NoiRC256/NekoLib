namespace NekoSystems.UI
{
    public interface IPanelController : IScreenController
    {
        PanelPriority PanelPriority { get; }
    }
}