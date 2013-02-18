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
using System.Xml.Serialization;

namespace AvalonDock.Layout
{
    [Serializable]
    public abstract class LayoutGroup<T> : LayoutGroupBase, ILayoutContainer, ILayoutGroup, IXmlSerializable where T : class, ILayoutElement
    {
        internal LayoutGroup()
        {
            _children.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_children_CollectionChanged);
        }

        void _children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove ||
                e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                if (e.OldItems != null)
                {
                    foreach (LayoutElement element in e.OldItems)
                    {
                        if (element.Parent == this)
                            element.Parent = null;
                    }
                }
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add ||
                e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                if (e.NewItems != null)
                {
                    foreach (LayoutElement element in e.NewItems)
                    {
                        if (element.Parent != this)
                        {
                            if (element.Parent != null)
                                element.Parent.RemoveChild(element);
                            element.Parent = this;
                        }
                        
                    }
                }
            }

            ComputeVisibility();
            OnChildrenCollectionChanged();
            NotifyChildrenTreeChanged(ChildrenTreeChange.DirectChildrenChanged);
            RaisePropertyChanged("ChildrenCount");
        }

        ObservableCollection<T> _children = new ObservableCollection<T>();

        public ObservableCollection<T> Children
        {
            get { return _children; }
        }

        IEnumerable<ILayoutElement> ILayoutContainer.Children
        {
            get { return _children.Cast<ILayoutElement>(); }
        }


        #region IsVisible

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            protected set
            {
                if (_isVisible != value)
                {
                    RaisePropertyChanging("IsVisible");
                    _isVisible = value;
                    OnIsVisibleChanged();
                    RaisePropertyChanged("IsVisible");
                }
            }
        }

        protected virtual void OnIsVisibleChanged()
        {
            UpdateParentVisibility();
        }

        void UpdateParentVisibility()
        {
            var parentPane = Parent as ILayoutElementWithVisibility;
            if (parentPane != null)
                parentPane.ComputeVisibility();
        }


        public void ComputeVisibility()
        {
            IsVisible = GetVisibility();
        }

        protected abstract bool GetVisibility();

        protected override void OnParentChanged(ILayoutContainer oldValue, ILayoutContainer newValue)
        {
            base.OnParentChanged(oldValue, newValue);

            ComputeVisibility();
        }

        #endregion


        public void MoveChild(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex)
                return;
            _children.Move(oldIndex, newIndex);
            ChildMoved(oldIndex, newIndex);
        }

        protected virtual void ChildMoved(int oldIndex, int newIndex)
        { 
        
        }

        public void RemoveChildAt(int childIndex)
        {
            _children.RemoveAt(childIndex);
        }

        public int IndexOfChild(ILayoutElement element)
        {
            return _children.Cast<ILayoutElement>().ToList().IndexOf(element);
        }

        public void InsertChildAt(int index, ILayoutElement element)
        {
            _children.Insert(index, (T)element);
        }

        public void RemoveChild(ILayoutElement element)
        {
            _children.Remove((T)element);
        }

        public void ReplaceChild(ILayoutElement oldElement, ILayoutElement newElement)
        {
            int index = _children.IndexOf((T)oldElement);
            _children.Insert(index, (T)newElement);
            _children.RemoveAt(index + 1);
        }

        public int ChildrenCount
        {
            get { return _children.Count; }
        }

        public void ReplaceChildAt(int index, ILayoutElement element)
        {
            _children[index] = (T)element;
        }


        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.IsEmptyElement)
            {
                reader.Read();
                ComputeVisibility();
                return;
            }
            string localName = reader.LocalName;
            reader.Read();
            while (true)
            {
                if (reader.LocalName == localName &&
                    reader.NodeType == System.Xml.XmlNodeType.EndElement)
                {
                    break;
                }

                XmlSerializer serializer = null;
                if (reader.LocalName == "LayoutAnchorablePaneGroup")
                    serializer = new XmlSerializer(typeof(LayoutAnchorablePaneGroup));
                else if (reader.LocalName == "LayoutAnchorablePane")
                    serializer = new XmlSerializer(typeof(LayoutAnchorablePane));
                else if (reader.LocalName == "LayoutAnchorable")
                    serializer = new XmlSerializer(typeof(LayoutAnchorable));
                else if (reader.LocalName == "LayoutDocumentPaneGroup")
                    serializer = new XmlSerializer(typeof(LayoutDocumentPaneGroup));
                else if (reader.LocalName == "LayoutDocumentPane")
                    serializer = new XmlSerializer(typeof(LayoutDocumentPane));
                else if (reader.LocalName == "LayoutDocument")
                    serializer = new XmlSerializer(typeof(LayoutDocument));
                else if (reader.LocalName == "LayoutAnchorGroup")
                    serializer = new XmlSerializer(typeof(LayoutAnchorGroup));
                else if (reader.LocalName == "LayoutPanel")
                    serializer = new XmlSerializer(typeof(LayoutPanel));

                Children.Add((T)serializer.Deserialize(reader));
            }

            reader.ReadEndElement();
        }

        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (var child in Children)
            {
                var type = child.GetType();
                XmlSerializer serializer = new XmlSerializer(type);
                serializer.Serialize(writer, child);
            }

        }
    }
}
