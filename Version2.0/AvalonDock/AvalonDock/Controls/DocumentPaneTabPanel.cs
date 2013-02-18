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
    public class DocumentPaneTabPanel : Panel
    {
        public DocumentPaneTabPanel()
        {
            FlowDirection = System.Windows.FlowDirection.LeftToRight;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var visibleChildren = Children.Cast<UIElement>().Where(ch => ch.Visibility != System.Windows.Visibility.Collapsed); 
            
            Size desideredSize = new Size();
            foreach (FrameworkElement child in Children)
            {
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    desideredSize.Width += child.DesiredSize.Width;

                desideredSize.Height = Math.Max(desideredSize.Height, child.DesiredSize.Height);
            }

            return new Size(Math.Min(desideredSize.Width, availableSize.Width), desideredSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            restart:
            var visibleChildren = Children.Cast<UIElement>().Where(ch => ch.Visibility != System.Windows.Visibility.Collapsed);
            double offset = 0.0;
            bool skipAllOthers = false;
            foreach (TabItem doc in visibleChildren)
            {
                var layoutContent = doc.Content as LayoutContent;
                if (skipAllOthers || offset + doc.DesiredSize.Width > finalSize.Width)
                {
                    if (layoutContent.IsSelected)
                    {
                        var parentContainer = layoutContent.Parent as ILayoutContainer;
                        var parentSelector = layoutContent.Parent as ILayoutContentSelector;
                        var parentPane = layoutContent.Parent as ILayoutPane;
                        int contentIndex = parentSelector.IndexOf(layoutContent);
                        if (contentIndex > 0 &&
                            parentContainer.ChildrenCount > 1)
                        {
                            parentPane.MoveChild(contentIndex, 0);
                            parentSelector.SelectedContentIndex = 0;
                            goto restart;
                        }
                    }
                    doc.Visibility = System.Windows.Visibility.Hidden;
                    skipAllOthers = true;
                }
                else
                {
                    doc.Visibility = System.Windows.Visibility.Visible;
                    doc.Arrange(new Rect(offset, 0.0, doc.DesiredSize.Width, finalSize.Height));
                    offset += doc.ActualWidth + doc.Margin.Left + doc.Margin.Right;
                }
            }

            return finalSize;

        }


        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            //if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed &&
            //    LayoutDocumentTabItem.IsDraggingItem())
            //{
            //    var contentModel = LayoutDocumentTabItem.GetDraggingItem().Model;
            //    var manager = contentModel.Root.Manager;
            //    LayoutDocumentTabItem.ResetDraggingItem();
            //    System.Diagnostics.Debug.WriteLine("OnMouseLeave()");

                
            //    manager.StartDraggingFloatingWindowForContent(contentModel);
            //}

            base.OnMouseLeave(e);
        }
    }
}
