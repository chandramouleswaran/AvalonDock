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
using System.Threading;

namespace AvalonDock.Controls
{
    class FocusChangeEventArgs : EventArgs
    {
        public FocusChangeEventArgs(IntPtr gotFocusWinHandle, IntPtr lostFocusWinHandle)
        {
            GotFocusWinHandle = gotFocusWinHandle;
            LostFocusWinHandle = lostFocusWinHandle;
        }

        public IntPtr GotFocusWinHandle
        {
            get;
            private set;
        }
        public IntPtr LostFocusWinHandle
        {
            get;
            private set;
        }
    }

    class WindowHookHandler
    {
        public WindowHookHandler()
        { 
        
        }
        
        IntPtr _windowHook;
        Win32Helper.HookProc _hookProc;
        public void Attach()
        {
            _hookProc = new Win32Helper.HookProc(this.HookProc);
            _windowHook = Win32Helper.SetWindowsHookEx(
                Win32Helper.HookType.WH_CBT,
                _hookProc,
                IntPtr.Zero,
                (int)Win32Helper.GetCurrentThreadId());
        }


        public void Detach()
        {
            Win32Helper.UnhookWindowsHookEx(_windowHook);
        }   

        public int HookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code == Win32Helper.HCBT_SETFOCUS)
            {
                if (FocusChanged != null)
                    FocusChanged(this, new FocusChangeEventArgs(wParam, lParam));
            }
            else if (code == Win32Helper.HCBT_ACTIVATE)
            {
                if (_insideActivateEvent.CanEnter)
                {
                    using (_insideActivateEvent.Enter())
                    {
                        //if (Activate != null)
                        //    Activate(this, new WindowActivateEventArgs(wParam));
                    }
                }
            }
            

            return Win32Helper.CallNextHookEx(_windowHook, code, wParam, lParam);
        }

        public event EventHandler<FocusChangeEventArgs> FocusChanged;

        //public event EventHandler<WindowActivateEventArgs> Activate;

        ReentrantFlag _insideActivateEvent = new ReentrantFlag();
    }
}
