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
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Diagnostics;

namespace AvalonDock.Controls
{
    public class DropDownButton : ToggleButton
    {
        public DropDownButton()
        {
            this.Unloaded += new RoutedEventHandler(DropDownButton_Unloaded);
        }


        #region DropDownContextMenu

        /// <summary>
        /// DropDownContextMenu Dependency Property
        /// </summary>
        public static readonly DependencyProperty DropDownContextMenuProperty =
            DependencyProperty.Register("DropDownContextMenu", typeof(ContextMenu), typeof(DropDownButton),
                new FrameworkPropertyMetadata((ContextMenu)null,
                    new PropertyChangedCallback(OnDropDownContextMenuChanged)));

        /// <summary>
        /// Gets or sets the DropDownContextMenu property.  This dependency property 
        /// indicates drop down menu to show up when user click on an anchorable menu pin.
        /// </summary>
        public ContextMenu DropDownContextMenu
        {
            get { return (ContextMenu)GetValue(DropDownContextMenuProperty); }
            set { SetValue(DropDownContextMenuProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DropDownContextMenu property.
        /// </summary>
        private static void OnDropDownContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DropDownButton)d).OnDropDownContextMenuChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DropDownContextMenu property.
        /// </summary>
        protected virtual void OnDropDownContextMenuChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldContextMenu = e.OldValue as ContextMenu;
            if (oldContextMenu != null && IsChecked.GetValueOrDefault())
                oldContextMenu.Closed -= new RoutedEventHandler(OnContextMenuClosed);
        }

        #endregion

        #region DropDownContextMenuDataContext

        /// <summary>
        /// DropDownContextMenuDataContext Dependency Property
        /// </summary>
        public static readonly DependencyProperty DropDownContextMenuDataContextProperty =
            DependencyProperty.Register("DropDownContextMenuDataContext", typeof(object), typeof(DropDownButton),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets or sets the DropDownContextMenuDataContext property.  This dependency property 
        /// indicates data context to set for drop down context menu.
        /// </summary>
        public object DropDownContextMenuDataContext
        {
            get { return (object)GetValue(DropDownContextMenuDataContextProperty); }
            set { SetValue(DropDownContextMenuDataContextProperty, value); }
        }

        #endregion

        protected override void OnClick()
        {
            if (DropDownContextMenu != null)
            {
                //IsChecked = true;
                DropDownContextMenu.PlacementTarget = this;
                DropDownContextMenu.Placement = PlacementMode.Bottom;
                DropDownContextMenu.DataContext = DropDownContextMenuDataContext;
                DropDownContextMenu.Closed += new RoutedEventHandler(OnContextMenuClosed);
                DropDownContextMenu.IsOpen = true;
            }

            base.OnClick();
        }

        void OnContextMenuClosed(object sender, RoutedEventArgs e)
        {
            //Debug.Assert(IsChecked.GetValueOrDefault());
            var ctxMenu = sender as ContextMenu;
            ctxMenu.Closed -= new RoutedEventHandler(OnContextMenuClosed);
            IsChecked = false;
        }

        void DropDownButton_Unloaded(object sender, RoutedEventArgs e)
        {
            DropDownContextMenu = null;
        }
        

    }
}
