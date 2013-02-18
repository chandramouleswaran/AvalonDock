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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;

namespace AvalonDock.Controls
{
    public class DropDownControlArea : UserControl
    {
        //static DropDownControlArea()
        //{
        //    //IsHitTestVisibleProperty.OverrideMetadata(typeof(DropDownControlArea), new FrameworkPropertyMetadata(true));
        //}
        
        public DropDownControlArea()
        { 
        
        }

        #region DropDownContextMenu

        /// <summary>
        /// DropDownContextMenu Dependency Property
        /// </summary>
        public static readonly DependencyProperty DropDownContextMenuProperty =
            DependencyProperty.Register("DropDownContextMenu", typeof(ContextMenu), typeof(DropDownControlArea),
                new FrameworkPropertyMetadata((ContextMenu)null));

        /// <summary>
        /// Gets or sets the DropDownContextMenu property.  This dependency property 
        /// indicates context menu to show when a right click is detected over the area occpied by the control.
        /// </summary>
        public ContextMenu DropDownContextMenu
        {
            get { return (ContextMenu)GetValue(DropDownContextMenuProperty); }
            set { SetValue(DropDownContextMenuProperty, value); }
        }

        #endregion

        #region DropDownContextMenuDataContext

        /// <summary>
        /// DropDownContextMenuDataContext Dependency Property
        /// </summary>
        public static readonly DependencyProperty DropDownContextMenuDataContextProperty =
            DependencyProperty.Register("DropDownContextMenuDataContext", typeof(object), typeof(DropDownControlArea),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets or sets the DropDownContextMenuDataContext property.  This dependency property 
        /// indicates data context to attach when context menu is shown.
        /// </summary>
        public object DropDownContextMenuDataContext
        {
            get { return (object)GetValue(DropDownContextMenuDataContextProperty); }
            set { SetValue(DropDownContextMenuDataContextProperty, value); }
        }

        #endregion

        protected override void OnMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);


        }

        protected override void OnPreviewMouseRightButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonUp(e);
            
            if (!e.Handled)
            {
                if (DropDownContextMenu != null)
                {
                    DropDownContextMenu.PlacementTarget = null;
                    DropDownContextMenu.Placement = PlacementMode.MousePoint;
                    DropDownContextMenu.DataContext = DropDownContextMenuDataContext;
                    DropDownContextMenu.IsOpen = true;
                   // e.Handled = true;
                }
            } 
        }


        //protected override System.Windows.Media.HitTestResult HitTestCore(System.Windows.Media.PointHitTestParameters hitTestParameters)
        //{
        //    var hitResult = base.HitTestCore(hitTestParameters);
        //    if (hitResult == null)
        //        return new PointHitTestResult(this, hitTestParameters.HitPoint);
            
        //    return hitResult;
        //}
    }
}
