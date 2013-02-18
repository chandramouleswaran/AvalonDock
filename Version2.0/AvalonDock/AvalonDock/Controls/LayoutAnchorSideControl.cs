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
using System.Collections.ObjectModel;
using AvalonDock.Layout;

namespace AvalonDock.Controls
{
    public class LayoutAnchorSideControl : Control, ILayoutControl
    {
        static LayoutAnchorSideControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutAnchorSideControl), new FrameworkPropertyMetadata(typeof(LayoutAnchorSideControl)));
        }


        internal LayoutAnchorSideControl(LayoutAnchorSide model)
        {
            if (model == null)
                throw new ArgumentNullException("model");


            _model = model;

            CreateChildrenViews();

            _model.Children.CollectionChanged += (s, e) => OnModelChildrenCollectionChanged(e);

            UpdateSide();
        }

        private void CreateChildrenViews()
        {
            var manager = _model.Root.Manager;
            foreach (var childModel in _model.Children)
            {
                _childViews.Add(manager.CreateUIElementForModel(childModel) as LayoutAnchorGroupControl);
            }
        }

        private void OnModelChildrenCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null && 
                (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove || 
                e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace))
            {
                foreach (var childModel in e.OldItems)
                    _childViews.Remove(_childViews.First(cv => cv.Model == childModel));
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                _childViews.Clear();

            if (e.NewItems != null &&
                (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add ||
                e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace))
            {
                var manager = _model.Root.Manager;
                int insertIndex = e.NewStartingIndex;
                foreach (LayoutAnchorGroup childModel in e.NewItems)
                {
                    _childViews.Insert(insertIndex++, manager.CreateUIElementForModel(childModel) as LayoutAnchorGroupControl);
                }
            }
        }

        LayoutAnchorSide _model = null;
        public ILayoutElement Model
        {
            get { return _model; }
        }

        ObservableCollection<LayoutAnchorGroupControl> _childViews = new ObservableCollection<LayoutAnchorGroupControl>();

        public ObservableCollection<LayoutAnchorGroupControl> Children
        {
            get { return _childViews; }
        }

        void UpdateSide()
        {
            switch (_model.Side)
            {
                case AnchorSide.Left:
                    SetIsLeftSide(true);
                    break;
                case AnchorSide.Top:
                    SetIsTopSide(true);
                    break;
                case AnchorSide.Right:
                    SetIsRightSide(true);
                    break;
                case AnchorSide.Bottom:
                    SetIsBottomSide(true);
                    break;
            }
        }

        #region IsLeftSide

        /// <summary>
        /// IsLeftSide Read-Only Dependency Property
        /// </summary>
        private static readonly DependencyPropertyKey IsLeftSidePropertyKey
            = DependencyProperty.RegisterReadOnly("IsLeftSide", typeof(bool), typeof(LayoutAnchorSideControl),
                new FrameworkPropertyMetadata((bool)false));

        public static readonly DependencyProperty IsLeftSideProperty
            = IsLeftSidePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the IsLeftSide property.  This dependency property 
        /// indicates this control is anchored to left side.
        /// </summary>
        public bool IsLeftSide
        {
            get { return (bool)GetValue(IsLeftSideProperty); }
        }

        /// <summary>
        /// Provides a secure method for setting the IsLeftSide property.  
        /// This dependency property indicates this control is anchored to left side.
        /// </summary>
        /// <param name="value">The new value for the property.</param>
        protected void SetIsLeftSide(bool value)
        {
            SetValue(IsLeftSidePropertyKey, value);
        }

        #endregion

        #region IsTopSide

        /// <summary>
        /// IsTopSide Read-Only Dependency Property
        /// </summary>
        private static readonly DependencyPropertyKey IsTopSidePropertyKey
            = DependencyProperty.RegisterReadOnly("IsTopSide", typeof(bool), typeof(LayoutAnchorSideControl),
                new FrameworkPropertyMetadata((bool)false));

        public static readonly DependencyProperty IsTopSideProperty
            = IsTopSidePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the IsTopSide property.  This dependency property 
        /// indicates this control is anchored to top side.
        /// </summary>
        public bool IsTopSide
        {
            get { return (bool)GetValue(IsTopSideProperty); }
        }

        /// <summary>
        /// Provides a secure method for setting the IsTopSide property.  
        /// This dependency property indicates this control is anchored to top side.
        /// </summary>
        /// <param name="value">The new value for the property.</param>
        protected void SetIsTopSide(bool value)
        {
            SetValue(IsTopSidePropertyKey, value);
        }

        #endregion

        #region IsRightSide

        /// <summary>
        /// IsRightSide Read-Only Dependency Property
        /// </summary>
        private static readonly DependencyPropertyKey IsRightSidePropertyKey
            = DependencyProperty.RegisterReadOnly("IsRightSide", typeof(bool), typeof(LayoutAnchorSideControl),
                new FrameworkPropertyMetadata((bool)false));

        public static readonly DependencyProperty IsRightSideProperty
            = IsRightSidePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the IsRightSide property.  This dependency property 
        /// indicates this control is anchored to right side.
        /// </summary>
        public bool IsRightSide
        {
            get { return (bool)GetValue(IsRightSideProperty); }
        }

        /// <summary>
        /// Provides a secure method for setting the IsRightSide property.  
        /// This dependency property indicates this control is anchored to right side.
        /// </summary>
        /// <param name="value">The new value for the property.</param>
        protected void SetIsRightSide(bool value)
        {
            SetValue(IsRightSidePropertyKey, value);
        }

        #endregion

        #region IsBottomSide

        /// <summary>
        /// IsBottomSide Read-Only Dependency Property
        /// </summary>
        private static readonly DependencyPropertyKey IsBottomSidePropertyKey
            = DependencyProperty.RegisterReadOnly("IsBottomSide", typeof(bool), typeof(LayoutAnchorSideControl),
                new FrameworkPropertyMetadata((bool)false));

        public static readonly DependencyProperty IsBottomSideProperty
            = IsBottomSidePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the IsBottomSide property.  This dependency property 
        /// indicates if this panel is anchored to bottom side.
        /// </summary>
        public bool IsBottomSide
        {
            get { return (bool)GetValue(IsBottomSideProperty); }
        }

        /// <summary>
        /// Provides a secure method for setting the IsBottomSide property.  
        /// This dependency property indicates if this panel is anchored to bottom side.
        /// </summary>
        /// <param name="value">The new value for the property.</param>
        protected void SetIsBottomSide(bool value)
        {
            SetValue(IsBottomSidePropertyKey, value);
        }

        #endregion


    }
}
