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
using System.Windows.Markup;
using System.Diagnostics;
using System.Xml.Serialization;

namespace AvalonDock.Layout
{
    [Serializable]
    [ContentProperty("RootPanel")]
    public class LayoutAnchorableFloatingWindow : LayoutFloatingWindow, ILayoutElementWithVisibility
    {
        public LayoutAnchorableFloatingWindow()
        { 
        
        }

        #region RootPanel

        private LayoutAnchorablePaneGroup _rootPanel = null;
        public LayoutAnchorablePaneGroup RootPanel
        {
            get { return _rootPanel; }
            set
            {
                if (_rootPanel != value)
                {
                    RaisePropertyChanging("RootPanel");

                    if (_rootPanel != null)
                        _rootPanel.ChildrenTreeChanged -= new EventHandler<ChildrenTreeChangedEventArgs>(_rootPanel_ChildrenTreeChanged);

                    _rootPanel = value;
                    if (_rootPanel != null)
                        _rootPanel.Parent = this;

                    if (_rootPanel != null)
                        _rootPanel.ChildrenTreeChanged += new EventHandler<ChildrenTreeChangedEventArgs>(_rootPanel_ChildrenTreeChanged);

                    RaisePropertyChanged("RootPanel");
                    RaisePropertyChanged("IsSinglePane");
                    RaisePropertyChanged("SinglePane");
                    RaisePropertyChanged("Children");
                    RaisePropertyChanged("ChildrenCount");
                    ((ILayoutElementWithVisibility)this).ComputeVisibility();
                }
            }
        }

        void _rootPanel_ChildrenTreeChanged(object sender, ChildrenTreeChangedEventArgs e)
        {
            RaisePropertyChanged("IsSinglePane");
            RaisePropertyChanged("SinglePane");
            
        }

        public bool IsSinglePane
        {
            get
            {
                return RootPanel != null && RootPanel.Descendents().OfType<ILayoutAnchorablePane>().Where(p => p.IsVisible).Count() == 1;
            }
        }

        public ILayoutAnchorablePane SinglePane
        {
            get
            {
                if (!IsSinglePane)
                    return null;

                var singlePane = RootPanel.Descendents().OfType<LayoutAnchorablePane>().Single(p => p.IsVisible);
                singlePane.UpdateIsDirectlyHostedInFloatingWindow();
                return singlePane;
            }
        }

        #endregion

        public override IEnumerable<ILayoutElement> Children
        {
            get 
            {
                if (ChildrenCount == 1)    
                    yield return RootPanel;

                yield break;
            }
        }

        public override void RemoveChild(ILayoutElement element)
        {
            Debug.Assert(element == RootPanel && element != null);
            RootPanel = null;
        }

        public override void ReplaceChild(ILayoutElement oldElement, ILayoutElement newElement)
        {
            Debug.Assert(oldElement == RootPanel && oldElement != null);
            RootPanel = newElement as LayoutAnchorablePaneGroup;
        }

        public override int ChildrenCount
        {
            get 
            {
                if (RootPanel == null)
                    return 0;
                return 1; 
            }
        }

        #region IsVisible
        [NonSerialized]
        private bool _isVisible = true;

        [XmlIgnore]
        public bool IsVisible
        {
            get { return _isVisible; }
            private set
            {
                if (_isVisible != value)
                {
                    RaisePropertyChanging("IsVisible");
                    _isVisible = value;
                    RaisePropertyChanged("IsVisible");
                    if (IsVisibleChanged != null)
                        IsVisibleChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler IsVisibleChanged;

        #endregion

       
        void ILayoutElementWithVisibility.ComputeVisibility()
        {
            if (RootPanel != null)
                IsVisible = RootPanel.IsVisible;
            else
                IsVisible = false;

        }

        public override bool IsValid
        {
            get { return RootPanel != null; }
        }

#if DEBUG
        public override void ConsoleDump(int tab)
        {
            System.Diagnostics.Debug.Write(new string(' ', tab * 4));
            System.Diagnostics.Debug.WriteLine("FloatingAnchorableWindow()");

            RootPanel.ConsoleDump(tab + 1);
        }
#endif        
    }
}
