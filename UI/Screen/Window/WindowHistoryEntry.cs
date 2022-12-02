using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekoSystems.UI
{
    public class WindowHistoryEntry
    {
        public readonly IWindowController Window;
        public readonly IWindowProperties Properties;

        public WindowHistoryEntry(IWindowController window, IWindowProperties properties)
        {
            Window = window;
            Properties = properties;
        }

        public void Show()
        {
            Window.Show(Properties);
        }
    }
}