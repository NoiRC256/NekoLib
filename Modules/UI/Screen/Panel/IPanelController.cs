namespace Nep.UI
{
    public interface IPanelController : IController
    {
        PanelPriority PanelPriority { get; }
    }
}