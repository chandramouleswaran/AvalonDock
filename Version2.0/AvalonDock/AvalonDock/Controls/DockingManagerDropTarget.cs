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
using System.Windows.Media;
using AvalonDock.Layout;

namespace AvalonDock.Controls
{
    internal class DockingManagerDropTarget : DropTarget<DockingManager>
    {
        internal DockingManagerDropTarget(DockingManager manager, Rect detectionRect, DropTargetType type)
            : base(manager, detectionRect, type)
        {
            _manager = manager;
        }

        DockingManager _manager;

        protected override void Drop(LayoutAnchorableFloatingWindow floatingWindow)
        {
            switch (Type)
            {
                case DropTargetType.DockingManagerDockLeft:
                    #region DropTargetType.DockingManagerDockLeft
                    {
                        if (_manager.Layout.RootPanel.Orientation != System.Windows.Controls.Orientation.Horizontal &&
                            _manager.Layout.RootPanel.Children.Count == 1)
                            _manager.Layout.RootPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

                        if (_manager.Layout.RootPanel.Orientation == System.Windows.Controls.Orientation.Horizontal)
                        {
                            var layoutAnchorablePaneGroup = floatingWindow.RootPanel as LayoutAnchorablePaneGroup;
                            if (layoutAnchorablePaneGroup != null &&
                                layoutAnchorablePaneGroup.Orientation == System.Windows.Controls.Orientation.Horizontal)
                            {
                                var childrenToTransfer = layoutAnchorablePaneGroup.Children.ToArray();
                                for (int i = 0; i < childrenToTransfer.Length; i++)
                                    _manager.Layout.RootPanel.Children.Insert(i, childrenToTransfer[i]);
                            }
                            else
                                _manager.Layout.RootPanel.Children.Insert(0, floatingWindow.RootPanel);
                        }
                        else
                        {
                            var newOrientedPanel = new LayoutPanel()
                            {
                                Orientation = System.Windows.Controls.Orientation.Horizontal
                            };

                            newOrientedPanel.Children.Add(floatingWindow.RootPanel);
                            newOrientedPanel.Children.Add(_manager.Layout.RootPanel);

                            _manager.Layout.RootPanel = newOrientedPanel;
                        }
                    }
                    break;
                    #endregion
                case DropTargetType.DockingManagerDockRight:
                    #region DropTargetType.DockingManagerDockRight
                    {
                        if (_manager.Layout.RootPanel.Orientation != System.Windows.Controls.Orientation.Horizontal &&
                            _manager.Layout.RootPanel.Children.Count == 1)
                            _manager.Layout.RootPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

                        if (_manager.Layout.RootPanel.Orientation == System.Windows.Controls.Orientation.Horizontal)
                        {
                            var layoutAnchorablePaneGroup = floatingWindow.RootPanel as LayoutAnchorablePaneGroup;
                            if (layoutAnchorablePaneGroup != null &&
                                layoutAnchorablePaneGroup.Orientation == System.Windows.Controls.Orientation.Horizontal)
                            {
                                var childrenToTransfer = layoutAnchorablePaneGroup.Children.ToArray();
                                for (int i = 0; i < childrenToTransfer.Length; i++)
                                    _manager.Layout.RootPanel.Children.Add(childrenToTransfer[i]);
                            }
                            else
                                _manager.Layout.RootPanel.Children.Add(floatingWindow.RootPanel);
                        }
                        else
                        {
                            var newOrientedPanel = new LayoutPanel()
                            {
                                Orientation = System.Windows.Controls.Orientation.Horizontal
                            };

                            newOrientedPanel.Children.Add(_manager.Layout.RootPanel);
                            newOrientedPanel.Children.Add(floatingWindow.RootPanel);

                            _manager.Layout.RootPanel = newOrientedPanel;
                        }
                    }
                    break;
                    #endregion
                case DropTargetType.DockingManagerDockTop:
                    #region DropTargetType.DockingManagerDockTop
                    {
                        if (_manager.Layout.RootPanel.Orientation != System.Windows.Controls.Orientation.Vertical &&
                            _manager.Layout.RootPanel.Children.Count == 1)
                            _manager.Layout.RootPanel.Orientation = System.Windows.Controls.Orientation.Vertical;

                        if (_manager.Layout.RootPanel.Orientation == System.Windows.Controls.Orientation.Vertical)
                        {
                            var layoutAnchorablePaneGroup = floatingWindow.RootPanel as LayoutAnchorablePaneGroup;
                            if (layoutAnchorablePaneGroup != null &&
                                layoutAnchorablePaneGroup.Orientation == System.Windows.Controls.Orientation.Vertical)
                            {
                                var childrenToTransfer = layoutAnchorablePaneGroup.Children.ToArray();
                                for (int i = 0; i < childrenToTransfer.Length; i++)
                                    _manager.Layout.RootPanel.Children.Insert(i, childrenToTransfer[i]);
                            }
                            else
                                _manager.Layout.RootPanel.Children.Insert(0, floatingWindow.RootPanel);
                        }
                        else
                        {
                            var newOrientedPanel = new LayoutPanel()
                            {
                                Orientation = System.Windows.Controls.Orientation.Vertical
                            };

                            newOrientedPanel.Children.Add(floatingWindow.RootPanel);
                            newOrientedPanel.Children.Add(_manager.Layout.RootPanel);

                            _manager.Layout.RootPanel = newOrientedPanel;
                        }
                    }
                    break;
                    #endregion
                case DropTargetType.DockingManagerDockBottom:
                    #region DropTargetType.DockingManagerDockBottom
                    {
                        if (_manager.Layout.RootPanel.Orientation != System.Windows.Controls.Orientation.Vertical &&
                            _manager.Layout.RootPanel.Children.Count == 1)
                            _manager.Layout.RootPanel.Orientation = System.Windows.Controls.Orientation.Vertical;

                        if (_manager.Layout.RootPanel.Orientation == System.Windows.Controls.Orientation.Vertical)
                        {
                            var layoutAnchorablePaneGroup = floatingWindow.RootPanel as LayoutAnchorablePaneGroup;
                            if (layoutAnchorablePaneGroup != null &&
                                layoutAnchorablePaneGroup.Orientation == System.Windows.Controls.Orientation.Vertical)
                            {
                                var childrenToTransfer = layoutAnchorablePaneGroup.Children.ToArray();
                                for (int i = 0; i < childrenToTransfer.Length; i++)
                                    _manager.Layout.RootPanel.Children.Add(childrenToTransfer[i]);

                            }
                            else
                                _manager.Layout.RootPanel.Children.Add(floatingWindow.RootPanel);
                        }
                        else
                        {
                            var newOrientedPanel = new LayoutPanel()
                            {
                                Orientation = System.Windows.Controls.Orientation.Vertical
                            };

                            newOrientedPanel.Children.Add(_manager.Layout.RootPanel);
                            newOrientedPanel.Children.Add(floatingWindow.RootPanel);

                            _manager.Layout.RootPanel = newOrientedPanel;
                        }
                    }
                    break;
                    #endregion
            }


            base.Drop(floatingWindow);
        }

        public override System.Windows.Media.Geometry GetPreviewPath(OverlayWindow overlayWindow, LayoutFloatingWindow floatingWindowModel)
        {
            var anchorableFloatingWindowModel = floatingWindowModel as LayoutAnchorableFloatingWindow;
            var layoutAnchorablePane = anchorableFloatingWindowModel.RootPanel as ILayoutPositionableElement;
            var layoutAnchorablePaneWithActualSize = anchorableFloatingWindowModel.RootPanel as ILayoutPositionableElementWithActualSize;

            var targetScreenRect = TargetElement.GetScreenArea();

            switch (Type)
            {
                case DropTargetType.DockingManagerDockLeft:
                    {
                        var desideredWidth = layoutAnchorablePane.DockWidth.IsAbsolute ? layoutAnchorablePane.DockWidth.Value : layoutAnchorablePaneWithActualSize.ActualWidth;
                        var previewBoxRect = new Rect(
                            targetScreenRect.Left - overlayWindow.Left,
                            targetScreenRect.Top - overlayWindow.Top,
                            Math.Min(desideredWidth, targetScreenRect.Width / 2.0),
                            targetScreenRect.Height);

                        return new RectangleGeometry(previewBoxRect);
                    }
                case DropTargetType.DockingManagerDockTop:
                    {
                        var desideredHeight = layoutAnchorablePane.DockHeight.IsAbsolute ? layoutAnchorablePane.DockHeight.Value : layoutAnchorablePaneWithActualSize.ActualHeight;
                        var previewBoxRect = new Rect(
                            targetScreenRect.Left - overlayWindow.Left,
                            targetScreenRect.Top - overlayWindow.Top,
                            targetScreenRect.Width,
                            Math.Min(desideredHeight, targetScreenRect.Height / 2.0));

                        return new RectangleGeometry(previewBoxRect);
                    }
                case DropTargetType.DockingManagerDockRight:
                    {
                        var desideredWidth = layoutAnchorablePane.DockWidth.IsAbsolute ? layoutAnchorablePane.DockWidth.Value : layoutAnchorablePaneWithActualSize.ActualWidth;
                        var previewBoxRect = new Rect(
                            targetScreenRect.Right - overlayWindow.Left - Math.Min(desideredWidth, targetScreenRect.Width / 2.0),
                            targetScreenRect.Top - overlayWindow.Top,
                            Math.Min(desideredWidth, targetScreenRect.Width / 2.0),
                            targetScreenRect.Height);

                        return new RectangleGeometry(previewBoxRect);
                    }
                case DropTargetType.DockingManagerDockBottom:
                    {
                        var desideredHeight = layoutAnchorablePane.DockHeight.IsAbsolute ? layoutAnchorablePane.DockHeight.Value : layoutAnchorablePaneWithActualSize.ActualHeight;
                        var previewBoxRect = new Rect(
                            targetScreenRect.Left - overlayWindow.Left,
                            targetScreenRect.Bottom - overlayWindow.Top - Math.Min(desideredHeight, targetScreenRect.Height / 2.0),
                            targetScreenRect.Width,
                            Math.Min(desideredHeight, targetScreenRect.Height / 2.0));

                        return new RectangleGeometry(previewBoxRect);
                    }
            }


            throw new InvalidOperationException();
        }
    }
}
