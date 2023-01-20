using System;

namespace NekoLib.UI
{
    public class UISignalScreenEvtArgs : EventArgs
    {
        public string ScreenId { get; }
        public bool ShouldAnimate { get; }
        public int LayerId { get; }

        public UISignalScreenEvtArgs(string screenId, bool shouldAnimate = true, int layerId = -1)
        {
            ScreenId = screenId;
            ShouldAnimate = shouldAnimate;
            LayerId = layerId;
        }
    }
}
