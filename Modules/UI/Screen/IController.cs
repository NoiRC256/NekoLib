using System;

namespace Nep.UI
{
    public interface IController
    {
        string ScreenId { get; set; }
        bool IsVisible { get; }

        Action<IController> TransitionInFinished { get; set; }
        Action<IController> TransitionOutFinished { get; set; }
        Action<IController> CloseRequest { get; set; }
        Action<IController> ScreenDestroyed { get; set; }

        void Show(IModel parameters = null);
        void Hide(bool animate = true);
    }
}