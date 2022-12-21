using Nap.UI;
using System;

namespace Assets.Nep.UI
{
    public class UIChangeScreenEvtArgs : EventArgs
    {
        public string ScreenId { get; }
        public bool ShouldAnimate { get; }

        public UIChangeScreenEvtArgs(string screenId, bool shouldAnimate = true)
        {
              ScreenId = screenId;
            ShouldAnimate = shouldAnimate;
        }
    }
}
