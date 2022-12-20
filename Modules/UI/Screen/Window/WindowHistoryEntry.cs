using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nep.UI
{
    public class WindowHistoryEntry
    {
        public readonly IWindowController Controller;
        public readonly IWindowContext Context;

        public WindowHistoryEntry(IWindowController windowController, IWindowContext windowContext)
        {
            Controller = windowController;
            Context = windowContext;
        }

        public void Show()
        {
            Controller.Show(Context);
        }
    }
}