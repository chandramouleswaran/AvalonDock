using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AvalonDock;

namespace Sample2
{
    public static class HelperFuncs
    {
        public static string LayoutToString(this DockingManager dockManager)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            dockManager.SaveLayout(sw);
            return sb.ToString();
        }

        public static void LayoutFromString(this DockingManager dockManager, string layoutXml)
        {
            StringReader sr = new StringReader(layoutXml);
            dockManager.RestoreLayout(sr);
        }

    }
}
