using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonDock.Controls
{
    class WindowActivateEventArgs : EventArgs
    {
        public WindowActivateEventArgs(IntPtr hwndActivating)
        {
            HwndActivating = hwndActivating;
        }

        public IntPtr HwndActivating
        {
            get;
            private set;
        }
    }
}
