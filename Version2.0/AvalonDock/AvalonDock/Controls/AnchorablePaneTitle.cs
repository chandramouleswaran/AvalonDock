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
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using AvalonDock.Layout;

namespace AvalonDock.Controls
{

    public class AnchorablePaneTitle : Control
    {
        static AnchorablePaneTitle()
        {
            IsHitTestVisibleProperty.OverrideMetadata(typeof(AnchorablePaneTitle), new FrameworkPropertyMetadata(true));
            FocusableProperty.OverrideMetadata(typeof(AnchorablePaneTitle), new FrameworkPropertyMetadata(false));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnchorablePaneTitle), new FrameworkPropertyMetadata(typeof(AnchorablePaneTitle)));
        }


        public AnchorablePaneTitle()
        { 
            
        }

        #region Model

        /// <summary>
        /// Model Dependency Property
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(LayoutAnchorable), typeof(AnchorablePaneTitle),
                new FrameworkPropertyMetadata((LayoutAnchorable)null, new PropertyChangedCallback(_OnModelChanged)));

        /// <summary>
        /// Gets or sets the Model property.  This dependency property 
        /// indicates model attached to this view.
        /// </summary>
        public LayoutAnchorable Model
        {
            get { return (LayoutAnchorable)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        static void _OnModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AnchorablePaneTitle)sender).OnModelChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Model property.
        /// </summary>
        protected virtual void OnModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Model != null)
                SetLayoutItem(Model.Root.Manager.GetLayoutItemFromModel(Model));
            else
                SetLayoutItem(null);
        }

        #endregion


        #region LayoutItem

        /// <summary>
        /// LayoutItem Read-Only Dependency Property
        /// </summary>
        private static readonly DependencyPropertyKey LayoutItemPropertyKey
            = DependencyProperty.RegisterReadOnly("LayoutItem", typeof(LayoutItem), typeof(AnchorablePaneTitle),
                new FrameworkPropertyMetadata((LayoutItem)null));

        public static readonly DependencyProperty LayoutItemProperty
            = LayoutItemPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the LayoutItem property.  This dependency property 
        /// indicates the LayoutItem attached to this tag item.
        /// </summary>
        public LayoutItem LayoutItem
        {
            get { return (LayoutItem)GetValue(LayoutItemProperty); }
        }

        /// <summary>
        /// Provides a secure method for setting the LayoutItem property.  
        /// This dependency property indicates the LayoutItem attached to this tag item.
        /// </summary>
        /// <param name="value">The new value for the property.</param>
        protected void SetLayoutItem(LayoutItem value)
        {
            SetValue(LayoutItemPropertyKey, value);
        }

        #endregion


        private void OnHide()
        {
            Model.Hide();
        }

        private void OnToggleAutoHide()
        {
            Model.ToggleAutoHide();
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _isMouseDown = false;
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (_isMouseDown && e.LeftButton == MouseButtonState.Pressed)
            {
                var pane = this.FindVisualAncestor<LayoutAnchorablePaneControl>();
                if (pane != null)
                {
                    var paneModel = pane.Model as LayoutAnchorablePane;
                    var manager = paneModel.Root.Manager;

                    manager.StartDraggingFloatingWindowForPane(paneModel);
                }
            }

            _isMouseDown = false;
        }

        bool _isMouseDown = false;
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (!e.Handled)
            {
                bool attachFloatingWindow = false;
                var parentFloatingWindow = Model.FindParent<LayoutAnchorableFloatingWindow>();
                if (parentFloatingWindow != null)
                {
                    attachFloatingWindow = parentFloatingWindow.Descendents().OfType<LayoutAnchorablePane>().Count() == 1;
                }

                if (attachFloatingWindow)
                {
                    //the pane is hosted inside a floating window that contains only an anchorable pane so drag the floating window itself
                    var floatingWndControl = Model.Root.Manager.FloatingWindows.Single(fwc => fwc.Model == parentFloatingWindow);
                    floatingWndControl.AttachDrag(false);
                }
                else
                    _isMouseDown = true;//normal drag
            }
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            _isMouseDown = false;
            base.OnMouseLeftButtonUp(e);

            if (Model != null)
                Model.IsActive = true;//FocusElementManager.SetFocusOnLastElement(Model);
        }
    }
}
