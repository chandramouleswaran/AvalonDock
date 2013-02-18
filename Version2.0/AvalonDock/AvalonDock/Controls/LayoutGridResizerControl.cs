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
using System.Windows.Media;

namespace AvalonDock.Controls
{
    public class LayoutGridResizerControl : Thumb
    {
        static LayoutGridResizerControl()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(typeof(LayoutGridResizerControl)));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
            VerticalAlignmentProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
            BackgroundProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(Brushes.Transparent));
            IsHitTestVisibleProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(true, null));
        }

       
        #region BackgroundWhileDragging

        /// <summary>
        /// BackgroundWhileDragging Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackgroundWhileDraggingProperty =
            DependencyProperty.Register("BackgroundWhileDragging", typeof(Brush), typeof(LayoutGridResizerControl),
                new FrameworkPropertyMetadata((Brush)Brushes.Black));

        /// <summary>
        /// Gets or sets the BackgroundWhileDragging property.  This dependency property 
        /// indicates ....
        /// </summary>
        public Brush BackgroundWhileDragging
        {
            get { return (Brush)GetValue(BackgroundWhileDraggingProperty); }
            set { SetValue(BackgroundWhileDraggingProperty, value); }
        }

        #endregion

        #region OpacityWhileDragging

        /// <summary>
        /// OpacityWhileDragging Dependency Property
        /// </summary>
        public static readonly DependencyProperty OpacityWhileDraggingProperty =
            DependencyProperty.Register("OpacityWhileDragging", typeof(double), typeof(LayoutGridResizerControl),
                new FrameworkPropertyMetadata((double)0.5));

        /// <summary>
        /// Gets or sets the OpacityWhileDragging property.  This dependency property 
        /// indicates ....
        /// </summary>
        public double OpacityWhileDragging
        {
            get { return (double)GetValue(OpacityWhileDraggingProperty); }
            set { SetValue(OpacityWhileDraggingProperty, value); }
        }

        #endregion
    }
}
