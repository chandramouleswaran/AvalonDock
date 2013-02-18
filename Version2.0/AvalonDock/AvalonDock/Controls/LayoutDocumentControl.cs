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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvalonDock.Layout;

namespace AvalonDock.Controls
{
    public class LayoutDocumentControl : Control
    {
        static LayoutDocumentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutDocumentControl), new FrameworkPropertyMetadata(typeof(LayoutDocumentControl)));
            FocusableProperty.OverrideMetadata(typeof(LayoutDocumentControl), new FrameworkPropertyMetadata(false));
        }

        public LayoutDocumentControl()
        {
            //SetBinding(FlowDirectionProperty, new Binding("Model.Root.Manager.FlowDirection") { Source = this });

        }


        #region Model

        /// <summary>
        /// Model Dependency Property
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(LayoutContent), typeof(LayoutDocumentControl),
                new FrameworkPropertyMetadata((LayoutContent)null,
                    new PropertyChangedCallback(OnModelChanged)));

        /// <summary>
        /// Gets or sets the Model property.  This dependency property 
        /// indicates the model attached to this view.
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
            ((LayoutDocumentControl)d).OnModelChanged(e);
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
            = DependencyProperty.RegisterReadOnly("LayoutItem", typeof(LayoutItem), typeof(LayoutDocumentControl),
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

        protected override void OnPreviewGotKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (Model != null)
                Model.IsActive = true;
            base.OnPreviewGotKeyboardFocus(e);
        }


    }
}
