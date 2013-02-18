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

namespace AvalonDock.Controls
{
    public class MenuItemEx : MenuItem
    {
        static MenuItemEx()
        {
            IconProperty.OverrideMetadata(typeof(MenuItemEx), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIconPropertyChanged)));
        }


        public MenuItemEx()
        {
        }

        #region IconTemplate

        /// <summary>
        /// IconTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(MenuItemEx),
                new FrameworkPropertyMetadata((DataTemplate)null,
                    new PropertyChangedCallback(OnIconTemplateChanged)));

        /// <summary>
        /// Gets or sets the IconTemplate property.  This dependency property 
        /// indicates the data template for the icon.
        /// </summary>
        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)GetValue(IconTemplateProperty); }
            set { SetValue(IconTemplateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IconTemplate property.
        /// </summary>
        private static void OnIconTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItemEx)d).OnIconTemplateChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IconTemplate property.
        /// </summary>
        protected virtual void OnIconTemplateChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateIcon();
        }

        #endregion

        #region IconTemplateSelector

        /// <summary>
        /// IconTemplateSelector Dependency Property
        /// </summary>
        public static readonly DependencyProperty IconTemplateSelectorProperty =
            DependencyProperty.Register("IconTemplateSelector", typeof(DataTemplateSelector), typeof(MenuItemEx),
                new FrameworkPropertyMetadata((DataTemplateSelector)null,
                    new PropertyChangedCallback(OnIconTemplateSelectorChanged)));

        /// <summary>
        /// Gets or sets the IconTemplateSelector property.  This dependency property 
        /// indicates the DataTemplateSelector for the Icon.
        /// </summary>
        public DataTemplateSelector IconTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(IconTemplateSelectorProperty); }
            set { SetValue(IconTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IconTemplateSelector property.
        /// </summary>
        private static void OnIconTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItemEx)d).OnIconTemplateSelectorChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IconTemplateSelector property.
        /// </summary>
        protected virtual void OnIconTemplateSelectorChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateIcon();
        }

        #endregion

        static void OnIconPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ((MenuItemEx)sender).UpdateIcon();
            }
        }

        bool _reentrantFlag = false;
        void UpdateIcon()
        {
            if (_reentrantFlag)
                return;
            _reentrantFlag = true;
            if (IconTemplateSelector != null)
            {
                var dataTemplateToUse = IconTemplateSelector.SelectTemplate(Icon, this);
                if (dataTemplateToUse != null)
                    Icon = dataTemplateToUse.LoadContent();
            }
            else if (IconTemplate != null)
                Icon = IconTemplate.LoadContent();
            _reentrantFlag = false;
        }
    }
}
