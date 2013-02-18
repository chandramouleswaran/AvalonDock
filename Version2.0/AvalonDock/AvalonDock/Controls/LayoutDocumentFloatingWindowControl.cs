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
using System.Windows.Data;
using AvalonDock.Layout;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Microsoft.Windows.Shell;

namespace AvalonDock.Controls
{
    public class LayoutDocumentFloatingWindowControl : LayoutFloatingWindowControl
    {
        static LayoutDocumentFloatingWindowControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutDocumentFloatingWindowControl), new FrameworkPropertyMetadata(typeof(LayoutDocumentFloatingWindowControl)));
        } 

        internal LayoutDocumentFloatingWindowControl(LayoutDocumentFloatingWindow model)
            :base(model)
        {
            _model = model;
        }


        LayoutDocumentFloatingWindow _model;

        public override ILayoutElement Model
        {
            get { return _model; }
        }

        public LayoutItem RootDocumentLayoutItem
        {
            get { return _model.Root.Manager.GetLayoutItemFromModel(_model.RootDocument); }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (_model.RootDocument == null)
            {
                InternalClose();
            }
            else
            {
                var manager = _model.Root.Manager;

                Content = manager.CreateUIElementForModel(_model.RootDocument);

                _model.RootDocumentChanged += new EventHandler(_model_RootDocumentChanged);
            }
        }

        void _model_RootDocumentChanged(object sender, EventArgs e)
        {
            if (_model.RootDocument == null)
            {
                InternalClose();
            }
        }

        protected override IntPtr FilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32Helper.WM_NCLBUTTONDOWN: //Left button down on title -> start dragging over docking manager
                    if (wParam.ToInt32() == Win32Helper.HT_CAPTION)
                    {
                        if (_model.RootDocument != null)
                            _model.RootDocument.IsActive = true;
                    }
                    break;
                case Win32Helper.WM_NCRBUTTONUP:
                    if (wParam.ToInt32() == Win32Helper.HT_CAPTION)
                    {
                        if (OpenContextMenu())
                            handled = true;
                        if (_model.Root.Manager.ShowSystemMenu)
                            WindowChrome.GetWindowChrome(this).ShowSystemMenu = !handled;
                        else
                            WindowChrome.GetWindowChrome(this).ShowSystemMenu = false;
                    }
                    break;

            }
               
            return base.FilterMessage(hwnd, msg, wParam, lParam, ref handled);
        }

        bool OpenContextMenu()
        {
            var ctxMenu = _model.Root.Manager.DocumentContextMenu;
            if (ctxMenu != null && RootDocumentLayoutItem != null)
            {
                ctxMenu.PlacementTarget = null;
                ctxMenu.Placement = PlacementMode.MousePoint;
                ctxMenu.DataContext = RootDocumentLayoutItem;
                ctxMenu.IsOpen = true;
                return true;
            }

            return false;
        }


        protected override void OnClosed(EventArgs e)
        {
            var root = Model.Root;
            root.Manager.RemoveFloatingWindow(this);
            root.CollectGarbage();
 
            base.OnClosed(e);
           
            if (!CloseInitiatedByUser)
            {
                root.FloatingWindows.Remove(_model);
            }

            _model.RootDocumentChanged -= new EventHandler(_model_RootDocumentChanged);
        }

    }
}
