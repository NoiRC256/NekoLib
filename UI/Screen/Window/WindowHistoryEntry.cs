using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nep.UI
{
    public class WindowHistoryEntry
    {
        public readonly IWindowController Controller;
        public readonly IWindowProperties Properties;

        public WindowHistoryEntry(IWindowController controller, IWindowProperties windowProperties)
        {
            Controller = controller;
            Properties = windowProperties;
        }

        public void Show()
        {
            Controller.Show(Properties);
        }
    }
}