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
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using AvalonDock.Layout;

namespace AvalonDock.Controls
{
    public class LayoutDocumentPaneControl : TabControl, ILayoutControl//, ILogicalChildrenContainer
    {
        static LayoutDocumentPaneControl()
        {
            FocusableProperty.OverrideMetadata(typeof(LayoutDocumentPaneControl), new FrameworkPropertyMetadata(false));
        }


        internal LayoutDocumentPaneControl(LayoutDocumentPane model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            SetBinding(ItemsSourceProperty, new Binding("Model.Children") { Source = this });
            SetBinding(FlowDirectionProperty, new Binding("Model.Root.Manager.FlowDirection") { Source = this });

            this.LayoutUpdated += new EventHandler(OnLayoutUpdated);
        }

        void OnLayoutUpdated(object sender, EventArgs e)
        {
            var modelWithAtcualSize = _model as ILayoutPositionableElementWithActualSize;
            modelWithAtcualSize.ActualWidth = ActualWidth;
            modelWithAtcualSize.ActualHeight = ActualHeight;
        }

        protected override void OnGotKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            System.Diagnostics.Debug.WriteLine("OnGotKeyboardFocus({0}, {1})", e.Source, e.NewFocus);


            //if (_model.SelectedContent != null)
            //    _model.SelectedContent.IsActive = true;

        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (_model.SelectedContent != null)
                _model.SelectedContent.IsActive = true;
        }

        List<object> _logicalChildren = new List<object>();

        protected override System.Collections.IEnumerator LogicalChildren
        {
            get
            {
                return _logicalChildren.GetEnumerator();
            }
        }

        LayoutDocumentPane _model;

        public ILayoutElement Model
        {
            get { return _model; }
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (!e.Handled && _model.SelectedContent != null)
                _model.SelectedContent.IsActive = true;
        }

        protected override void OnMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            if (!e.Handled && _model.SelectedContent != null)
                _model.SelectedContent.IsActive = true;

        }
    }
}
