using System;

namespace Nep.UI
{
    public interface IScreenController
    {
        string ScreenId { get; set; }
        bool IsVisible { get; }

        Action<IScreenController> TransitionInFinished { get; set; }
        Action<IScreenController> TransitionOutFinished { get; set; }
        Action<IScreenController> CloseRequest { get; set; }
        Action<IScreenController> ScreenDestroyed { get; set; }

        void Show(IScreenProperties parameters = null);
        void Hide(bool animate = true);
    }
}