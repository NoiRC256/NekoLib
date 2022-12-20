using System;

namespace Nep.UI
{
    public interface IUIController
    {
        string ScreenId { get; set; }
        bool IsVisible { get; }

        Action<IUIController> TransitionInFinished { get; set; }
        Action<IUIController> TransitionOutFinished { get; set; }
        Action<IUIController> CloseRequest { get; set; }
        Action<IUIController> ScreenDestroyed { get; set; }

        void Show(IUIControllerContext context = null);
        void Hide(bool animate = true);
    }
}