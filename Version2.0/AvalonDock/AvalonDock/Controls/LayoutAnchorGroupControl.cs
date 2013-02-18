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
using System.Collections.ObjectModel;
using AvalonDock.Layout;
using System.Windows;

namespace AvalonDock.Controls
{
    public class LayoutAnchorGroupControl : Control, ILayoutControl
    {
        static LayoutAnchorGroupControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutAnchorGroupControl), new FrameworkPropertyMetadata(typeof(LayoutAnchorGroupControl)));
        }


        internal LayoutAnchorGroupControl(LayoutAnchorGroup model)
        {
            _model = model;
            CreateChildrenViews();

            _model.Children.CollectionChanged += (s, e) => OnModelChildrenCollectionChanged(e);
        }

        private void CreateChildrenViews()
        {
            var manager = _model.Root.Manager;
            foreach (var childModel in _model.Children)
            {
                _childViews.Add(new LayoutAnchorControl(childModel) { Template = manager.AnchorTemplate });
            }
        }

        private void OnModelChildrenCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove ||
                e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                if (e.OldItems != null)
                {
                    {
                        foreach (var childModel in e.OldItems)
                            _childViews.Remove(_childViews.First(cv => cv.Model == childModel));
                    }
                }
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                _childViews.Clear();

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add ||
                e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                if (e.NewItems != null)
                {
                    var manager = _model.Root.Manager;
                    int insertIndex = e.NewStartingIndex;
                    foreach (LayoutAnchorable childModel in e.NewItems)
                    {
                        _childViews.Insert(insertIndex++, new LayoutAnchorControl(childModel) { Template = manager.AnchorTemplate });
                    }
                }
            }
        }

        ObservableCollection<LayoutAnchorControl> _childViews = new ObservableCollection<LayoutAnchorControl>();

        public ObservableCollection<LayoutAnchorControl> Children
        {
            get { return _childViews; }
        }


        LayoutAnchorGroup _model;
        public ILayoutElement Model
        {
            get { return _model; }
        }
    }
}
