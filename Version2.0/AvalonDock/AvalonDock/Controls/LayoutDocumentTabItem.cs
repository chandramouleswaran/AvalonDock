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
using System.Windows.Input;
using AvalonDock.Layout;
using System.Diagnostics;
using System.Windows.Media;

namespace AvalonDock.Controls
{
    public class LayoutDocumentTabItem : Control
    {
        static LayoutDocumentTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutDocumentTabItem), new FrameworkPropertyMetadata(typeof(LayoutDocumentTabItem)));
        }

        public LayoutDocumentTabItem()
        {
        }

        #region Model

        /// <summary>
        /// Model Dependency Property
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(LayoutContent), typeof(LayoutDocumentTabItem),
                new FrameworkPropertyMetadata((LayoutContent)null,
                    new PropertyChangedCallback(OnModelChanged)));

        /// <summary>
        /// Gets or sets the Model property.  This dependency property 
        /// indicates the layout content model attached to the tab item.
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
            ((LayoutDocumentTabItem)d).OnModelChanged(e);
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
            = DependencyProperty.RegisterReadOnly("LayoutItem", typeof(LayoutItem), typeof(LayoutDocumentTabItem),
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

        List<Rect> _otherTabsScreenArea = null;
        List<TabItem> _otherTabs = null;
        Rect _parentDocumentTabPanelScreenArea;
        DocumentPaneTabPanel _parentDocumentTabPanel;
        bool _isMouseDown = false;
        Point _mouseDownPoint;

        void UpdateDragDetails()
        {
            _parentDocumentTabPanel = this.FindLogicalAncestor<DocumentPaneTabPanel>();
            _parentDocumentTabPanelScreenArea = _parentDocumentTabPanel.GetScreenArea();
            _otherTabs = _parentDocumentTabPanel.Children.Cast<TabItem>().Where(ch =>
                ch.Visibility != System.Windows.Visibility.Collapsed).ToList();
            Rect currentTabScreenArea = this.FindLogicalAncestor<TabItem>().GetScreenArea();
            _otherTabsScreenArea = _otherTabs.Select(ti => 
                {
                    var screenArea = ti.GetScreenArea();
                    return new Rect(screenArea.Left, screenArea.Top, currentTabScreenArea.Width, screenArea.Height);
                }).ToList();
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            
            Model.IsActive = true;

            if (e.ClickCount == 1)
            {
                _mouseDownPoint = e.GetPosition(this);
                _isMouseDown = true;
            }
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isMouseDown)
            {
                Point ptMouseMove = e.GetPosition(this);

                if (Math.Abs(ptMouseMove.X - _mouseDownPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(ptMouseMove.Y - _mouseDownPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    UpdateDragDetails();
                    CaptureMouse();
                    _isMouseDown = false;
                }
            }

            if (IsMouseCaptured)
            {
                var mousePosInScreenCoord = this.PointToScreenDPI(e.GetPosition(this));
                if (!_parentDocumentTabPanelScreenArea.Contains(mousePosInScreenCoord))
                {
                    ReleaseMouseCapture();
                    var manager = Model.Root.Manager;
                    manager.StartDraggingFloatingWindowForContent(Model);
                }
                else
                {
                    int indexOfTabItemWithMouseOver = _otherTabsScreenArea.FindIndex(r => r.Contains(mousePosInScreenCoord));
                    if (indexOfTabItemWithMouseOver >= 0)
                    {
                        var targetModel = _otherTabs[indexOfTabItemWithMouseOver].Content as LayoutContent;
                        var container = Model.Parent as ILayoutContainer;
                        var containerPane = Model.Parent as ILayoutPane;
                        var childrenList = container.Children.ToList();
                        containerPane.MoveChild(childrenList.IndexOf(Model), childrenList.IndexOf(targetModel));
                        Model.IsActive = true;
                        _parentDocumentTabPanel.UpdateLayout();
                        UpdateDragDetails();
                    }
                }
            }

        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
                ReleaseMouseCapture();
            _isMouseDown = false;

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _isMouseDown = false;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            _isMouseDown = false;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                if (LayoutItem.CloseCommand.CanExecute(null))
                    LayoutItem.CloseCommand.Execute(null);
            }
            
            base.OnMouseDown(e);
        }
    }
}
