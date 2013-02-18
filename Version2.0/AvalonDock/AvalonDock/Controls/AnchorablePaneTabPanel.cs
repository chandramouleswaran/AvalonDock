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
using AvalonDock.Layout;

namespace AvalonDock.Controls
{
    public class AnchorablePaneTabPanel : Panel
    {
        public AnchorablePaneTabPanel()
        {
            FlowDirection = System.Windows.FlowDirection.LeftToRight;
        }


        protected override Size MeasureOverride(Size availableSize)
        {
            double totWidth = 0;
            double maxHeight = 0;
            var visibleChildren = Children.Cast<UIElement>().Where(ch => ch.Visibility != System.Windows.Visibility.Collapsed);
            foreach (FrameworkElement child in visibleChildren)
            {
                child.Measure(new Size(double.PositiveInfinity, availableSize.Height));
                totWidth += child.DesiredSize.Width;
                maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
            }

            if (totWidth > availableSize.Width)
            {
                double childFinalDesideredWidth = availableSize.Width / visibleChildren.Count();
                foreach (FrameworkElement child in visibleChildren)
                {
                    child.Measure(new Size(childFinalDesideredWidth, availableSize.Height));
                }
            }

            return new Size(Math.Min(availableSize.Width, totWidth), maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var visibleChildren = Children.Cast<UIElement>().Where(ch => ch.Visibility != System.Windows.Visibility.Collapsed);


            double finalWidth = finalSize.Width;
            double desideredWidth = visibleChildren.Sum(ch => ch.DesiredSize.Width);
            double offsetX = 0.0;

            if (finalWidth > desideredWidth)
            {
                foreach (FrameworkElement child in visibleChildren)
                {
                    double childFinalWidth = child.DesiredSize.Width ;
                    child.Arrange(new Rect(offsetX, 0, childFinalWidth, finalSize.Height));

                    offsetX += childFinalWidth;
                }
            }
            else
            {
                double childFinalWidth = finalWidth / visibleChildren.Count();
                foreach (FrameworkElement child in visibleChildren)
                {
                    child.Arrange(new Rect(offsetX, 0, childFinalWidth, finalSize.Height));

                    offsetX += childFinalWidth;
                }   
            }

            return finalSize;
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed &&
                LayoutAnchorableTabItem.IsDraggingItem())
            {
                var contentModel = LayoutAnchorableTabItem.GetDraggingItem().Model as LayoutAnchorable;
                var manager = contentModel.Root.Manager;
                LayoutAnchorableTabItem.ResetDraggingItem();

                manager.StartDraggingFloatingWindowForContent(contentModel);
            }

            base.OnMouseLeave(e);
        }
    }
}
