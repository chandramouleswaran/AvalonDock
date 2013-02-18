using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonDock.Layout
{
    interface ILayoutPreviousContainer
    {
        ILayoutContainer PreviousContainer { get; set; }

        string PreviousContainerId { get; set; }
    }
}
