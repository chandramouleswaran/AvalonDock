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
using AvalonDock.Layout;
using System.Windows;

namespace AvalonDock.Controls
{
    public class LayoutDocumentItem : LayoutItem
    {
        LayoutDocument _document;
        internal LayoutDocumentItem()
        {
            
        }

        internal override void Attach(LayoutContent model)
        {
            _document = model as LayoutDocument;
            base.Attach(model);
        }

        protected override void Close()
        {
            var dockingManager = _document.Root.Manager;
            dockingManager._ExecuteCloseCommand(_document);
        }

        #region Description

        /// <summary>
        /// Description Dependency Property
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(LayoutDocumentItem),
                new FrameworkPropertyMetadata((string)null,
                    new PropertyChangedCallback(OnDescriptionChanged)));

        /// <summary>
        /// Gets or sets the Description property.  This dependency property 
        /// indicates the description to display for the document item.
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Description property.
        /// </summary>
        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LayoutDocumentItem)d).OnDescriptionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Description property.
        /// </summary>
        protected virtual void OnDescriptionChanged(DependencyPropertyChangedEventArgs e)
        {
            _document.Description = (string)e.NewValue;
        }

        #endregion

        internal override void Detach()
        {
            _document = null;
            base.Detach();
        }
    }
}
