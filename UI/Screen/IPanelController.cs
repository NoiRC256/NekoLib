namespace Nep.UI
{
    public interface IPanelController : IScreenController
    {
        PanelPriority PanelPriority { get; }
    }
}