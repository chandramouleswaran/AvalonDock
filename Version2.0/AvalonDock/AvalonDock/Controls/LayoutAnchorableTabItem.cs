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
using System.Windows.Input;
using AvalonDock.Layout;
using System.Reflection;
using System.Diagnostics;

namespace AvalonDock.Controls
{
    public class LayoutAnchorableTabItem : Control
    {
        static LayoutAnchorableTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutAnchorableTabItem), new FrameworkPropertyMetadata(typeof(LayoutAnchorableTabItem)));
        }

        public LayoutAnchorableTabItem()
        {
        }



        #region Model

        /// <summary>
        /// Model Dependency Property
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(LayoutContent), typeof(LayoutAnchorableTabItem),
                new FrameworkPropertyMetadata((LayoutContent)null,
                    new PropertyChangedCallback(OnModelChanged)));

        /// <summary>
        /// Gets or sets the Model property.  This dependency property 
        /// indicates model attached to the anchorable tab item.
        /// </summary>
        public LayoutContent Model
        {
            get { return (LayoutContent)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Model property.
        /// </summary>
        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LayoutAnchorableTabItem)d).OnModelChanged(e);
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
            //UpdateLogicalParent();
        }

        #endregion

        #region LayoutItem

        /// <summary>
        /// LayoutItem Read-Only Dependency Property
        /// </summary>
        private static readonly DependencyPropertyKey LayoutItemPropertyKey
            = DependencyProperty.RegisterReadOnly("LayoutItem", typeof(LayoutItem), typeof(LayoutAnchorableTabItem),
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

        bool _isMouseDown = false;
        static LayoutAnchorableTabItem _draggingItem = null;

        internal static bool IsDraggingItem()
        {
            return _draggingItem != null;
        }

        internal static LayoutAnchorableTabItem GetDraggingItem()
        {
            return _draggingItem;
        }
        internal static void ResetDraggingItem()
        {
            _draggingItem = null;
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            _isMouseDown = true;
            _draggingItem = this;
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _isMouseDown = false;
                _draggingItem = null;
            }
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            _isMouseDown = false;

            base.OnMouseLeftButtonUp(e);

            Model.IsActive = true;
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (_isMouseDown && e.LeftButton == MouseButtonState.Pressed)
            {
                _draggingItem = this;
            }

            _isMouseDown = false;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (_draggingItem != null &&
                _draggingItem != this &&
                e.LeftButton == MouseButtonState.Pressed)
            {
                //Debug.WriteLine("Dragging item from {0} to {1}", _draggingItem, this);

                var model = Model;
                var container = model.Parent as ILayoutContainer;
                var containerPane = model.Parent as ILayoutPane;
                var childrenList = container.Children.ToList();
                containerPane.MoveChild(childrenList.IndexOf(_draggingItem.Model), childrenList.IndexOf(model));
            }
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);

        }
    }
}
