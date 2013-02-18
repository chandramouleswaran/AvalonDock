//Copyright (c) 2007-2012, Adolfo Marinucci
//All rights reserved.

//Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//following conditions are met:

//* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

//* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
//disclaimer in the documentation and/or other materials provided with the distribution.

//* Neither the name of Adolfo Marinucci nor the names of its contributors may be used to endorse or promote products
//derived from this software without specific prior written permission.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
//INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, 
//EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
//STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
//EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AvalonDock.Layout
{
    public static class Extentions
    {
        public static IEnumerable<ILayoutElement> Descendents(this ILayoutElement element)
        {
            var container = element as ILayoutContainer;
            if (container != null)
            {
                foreach (var childElement in container.Children)
                {
                    yield return childElement;
                    foreach (var childChildElement in childElement.Descendents())
                        yield return childChildElement;
                }
            }
        }

        public static T FindParent<T>(this ILayoutElement element) //where T : ILayoutContainer
        { 
            var parent = element.Parent;
            while (parent != null &&
                !(parent is T))
                parent = parent.Parent;


            return (T)parent;
        }

        public static ILayoutRoot GetRoot(this ILayoutElement element) //where T : ILayoutContainer
        {
            if (element is ILayoutRoot)
                return element as ILayoutRoot;

            var parent = element.Parent;
            while (parent != null &&
                !(parent is ILayoutRoot))
                parent = parent.Parent;

            return (ILayoutRoot)parent;
        }

        public static bool ContainsChildOfType<T>(this ILayoutContainer element)
        {
            foreach (var childElement in element.Descendents())
                if (childElement is T)
                    return true;

            return false;
        }

        public static bool ContainsChildOfType<T, S>(this ILayoutContainer container)
        {
            foreach (var childElement in container.Descendents())
                if (childElement is T || childElement is S)
                    return true;

            return false;
        }

        public static bool IsOfType<T, S>(this ILayoutContainer container)
        {
            return container is T || container is S;
        }

        public static AnchorSide GetSide(this ILayoutElement element)
        {
            var parentContainer = element.Parent as ILayoutOrientableGroup;
            if (parentContainer != null)
            {
                if (!parentContainer.ContainsChildOfType<LayoutDocumentPaneGroup, LayoutDocumentPane>())
                    return GetSide(parentContainer);

                foreach (var childElement in parentContainer.Children)
                {
                    if (childElement == element ||
                        childElement.Descendents().Contains(element))
                        return parentContainer.Orientation == System.Windows.Controls.Orientation.Horizontal ?
                            AnchorSide.Left : AnchorSide.Top;

                    var childElementAsContainer = childElement as ILayoutContainer;
                    if (childElementAsContainer != null &&
                        (childElementAsContainer.IsOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>() ||
                        childElementAsContainer.ContainsChildOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>()))
                    {
                        return parentContainer.Orientation == System.Windows.Controls.Orientation.Horizontal ?
                           AnchorSide.Right : AnchorSide.Bottom;
                    }
                }
            }

            Debug.Fail("Unable to find the side for an element, possible layout problem!");
            return AnchorSide.Right;
        }


        internal static void KeepInsideNearestMonitor(this ILayoutElementForFloatingWindow paneInsideFloatingWindow)
        {
            Win32Helper.RECT r = new Win32Helper.RECT();
            r.Left = (int)paneInsideFloatingWindow.FloatingLeft;
            r.Top = (int)paneInsideFloatingWindow.FloatingTop;
            r.Bottom = r.Top + (int)paneInsideFloatingWindow.FloatingHeight;
            r.Right = r.Left + (int)paneInsideFloatingWindow.FloatingWidth;

            uint MONITOR_DEFAULTTONEAREST = 0x00000002;
            uint MONITOR_DEFAULTTONULL = 0x00000000;

            System.IntPtr monitor = Win32Helper.MonitorFromRect(ref r, MONITOR_DEFAULTTONULL);
            if (monitor == System.IntPtr.Zero)
            {
                System.IntPtr nearestmonitor = Win32Helper.MonitorFromRect(ref r, MONITOR_DEFAULTTONEAREST);
                if (nearestmonitor != System.IntPtr.Zero)
                {
                    Win32Helper.MonitorInfo monitorInfo = new Win32Helper.MonitorInfo();
                    monitorInfo.Size = Marshal.SizeOf(monitorInfo);
                    Win32Helper.GetMonitorInfo(nearestmonitor, monitorInfo);

                    if (paneInsideFloatingWindow.FloatingLeft < monitorInfo.Work.Left)
                    {
                        paneInsideFloatingWindow.FloatingLeft = monitorInfo.Work.Left + 10;
                    }

                    if (paneInsideFloatingWindow.FloatingLeft + paneInsideFloatingWindow.FloatingWidth > monitorInfo.Work.Right)
                    {
                        paneInsideFloatingWindow.FloatingLeft = monitorInfo.Work.Right - (paneInsideFloatingWindow.FloatingWidth + 10);
                    }

                    if (paneInsideFloatingWindow.FloatingTop < monitorInfo.Work.Top)
                    {
                        paneInsideFloatingWindow.FloatingTop = monitorInfo.Work.Top + 10;
                    }

                    if (paneInsideFloatingWindow.FloatingTop + paneInsideFloatingWindow.FloatingHeight > monitorInfo.Work.Bottom)
                    {
                        paneInsideFloatingWindow.FloatingTop = monitorInfo.Work.Bottom - (paneInsideFloatingWindow.FloatingHeight + 10);
                    }
                }
            }

        }

    }
}
