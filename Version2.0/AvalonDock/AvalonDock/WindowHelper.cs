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
using System.Windows.Media;
using System.Windows;
using System.Windows.Interop;

namespace AvalonDock
{
    static class WindowHelper
    {
        public static bool IsAttachedToPresentationSource(this Visual element)
        {
            return PresentationSource.FromVisual(element as Visual) != null;
        }

        public static void SetParentToMainWindowOf(this Window window, Visual element)
        {
            var wndParent = Window.GetWindow(element);
            if (wndParent != null)
                window.Owner = wndParent;
            else
            {
                IntPtr parentHwnd;
                if (GetParentWindowHandle(element, out parentHwnd))
                    Win32Helper.SetOwner(new WindowInteropHelper(window).Handle, parentHwnd);
            }
        }

        public static IntPtr GetParentWindowHandle(this Window window)
        {
            if (window.Owner != null)
                return new WindowInteropHelper(window.Owner).Handle;
            else
                return Win32Helper.GetOwner(new WindowInteropHelper(window).Handle);
        }


        public static bool GetParentWindowHandle(this Visual element, out IntPtr hwnd)
        {
            hwnd = IntPtr.Zero;
            HwndSource wpfHandle = PresentationSource.FromVisual(element) as HwndSource;
            
            if (wpfHandle == null)
                return false;

            hwnd = Win32Helper.GetParent(wpfHandle.Handle);
            if (hwnd == IntPtr.Zero)
                hwnd = wpfHandle.Handle;
            return true;
        }

        public static void SetParentWindowToNull(this Window window)
        {
            if (window.Owner != null)
                window.Owner = null;
            else
            {
                Win32Helper.SetOwner(new WindowInteropHelper(window).Handle, IntPtr.Zero);
            }
        }
    }
}
