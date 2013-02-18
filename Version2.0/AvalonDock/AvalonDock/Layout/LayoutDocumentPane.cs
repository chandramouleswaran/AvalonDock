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
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace AvalonDock.Layout
{
    [ContentProperty("Children")]
    [Serializable]
    public class LayoutDocumentPane : LayoutPositionableGroup<LayoutContent>, ILayoutDocumentPane, ILayoutPositionableElement, ILayoutContentSelector, ILayoutPaneSerializable
    {
        public LayoutDocumentPane()
        {
        }
        public LayoutDocumentPane(LayoutContent firstChild)
        {
            Children.Add(firstChild);
        }

        protected override bool GetVisibility()
        {
            if (Parent is LayoutDocumentPaneGroup)
                return ChildrenCount > 0;
            
            return true;
        }

        #region SelectedContentIndex

        private int _selectedIndex = -1;
        public int SelectedContentIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value < 0 ||
                    value >= Children.Count)
                    value = -1;

                if (_selectedIndex != value)
                {
                    RaisePropertyChanging("SelectedContentIndex");
                    RaisePropertyChanging("SelectedContent");
                    if (_selectedIndex >= 0 &&
                        _selectedIndex < Children.Count)
                        Children[_selectedIndex].IsSelected = false;

                    _selectedIndex = value;

                    if (_selectedIndex >= 0 &&
                        _selectedIndex < Children.Count)
                        Children[_selectedIndex].IsSelected = true;

                    RaisePropertyChanged("SelectedContentIndex");
                    RaisePropertyChanged("SelectedContent");
                }
            }
        }

        protected override void ChildMoved(int oldIndex, int newIndex)
        {
            if (_selectedIndex == oldIndex)
            {
                RaisePropertyChanging("SelectedContentIndex");
                _selectedIndex = newIndex;
                RaisePropertyChanged("SelectedContentIndex");
            }


            base.ChildMoved(oldIndex, newIndex);
        }

        public LayoutContent SelectedContent
        {
            get { return _selectedIndex == -1 ? null : Children[_selectedIndex]; }
        }
        #endregion

        protected override void OnChildrenCollectionChanged()
        {
            if (SelectedContentIndex >= ChildrenCount)
                SelectedContentIndex = Children.Count - 1;
            if (SelectedContentIndex == -1 && ChildrenCount > 0)
            {
                if (Root == null)//if I'm not yet connected just switch to first document
                    SelectedContentIndex = 0;
                else
                {
                    var childrenToSelect = Children.OrderByDescending(c => c.LastActivationTimeStamp.GetValueOrDefault()).First();
                    SelectedContentIndex = Children.IndexOf(childrenToSelect);
                    childrenToSelect.IsActive = true;
                }
            }

            base.OnChildrenCollectionChanged();

            RaisePropertyChanged("ChildrenSorted");
        }

        public int IndexOf(LayoutContent content)
        {
            return Children.IndexOf(content);
        }

        protected override void OnIsVisibleChanged()
        {
            UpdateParentVisibility();
            base.OnIsVisibleChanged();
        }

        void UpdateParentVisibility()
        {
            var parentPane = Parent as ILayoutElementWithVisibility;
            if (parentPane != null)
                parentPane.ComputeVisibility();
        }

        public IEnumerable<LayoutContent> ChildrenSorted
        {
            get 
            {
                var listSorted = Children.ToList();
                listSorted.Sort();
                return listSorted;
            }
        }

        string _id;
        string ILayoutPaneSerializable.Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            if (_id != null)
                writer.WriteAttributeString("Id", _id);

            base.WriteXml(writer);
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.MoveToAttribute("Id"))
                _id = reader.Value;


            base.ReadXml(reader);
        }


#if DEBUG
        public override void ConsoleDump(int tab)
        {
            System.Diagnostics.Debug.Write(new string(' ', tab * 4));
            System.Diagnostics.Debug.WriteLine("DocumentPane()");

            foreach (LayoutElement child in Children)
                child.ConsoleDump(tab + 1);
        }
#endif
    
    }
}
