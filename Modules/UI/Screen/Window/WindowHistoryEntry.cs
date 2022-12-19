using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nep.UI
{
    public class WindowHistoryEntry
    {
        public readonly IWindowController Controller;
        public readonly IWindowModel Properties;

        public WindowHistoryEntry(IWindowController windowController, IWindowModel windowModel)
        {
            Controller = windowController;
            Properties = windowModel;
        }

        public void Show()
        {
            Controller.Show(Properties);
        }
    }
}