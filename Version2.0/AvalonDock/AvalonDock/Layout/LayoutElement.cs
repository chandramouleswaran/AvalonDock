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
using System.ComponentModel;
using System.Xml.Serialization;

namespace AvalonDock.Layout
{
    [Serializable]
    public abstract class LayoutElement : DependencyObject, ILayoutElement
    {
        internal LayoutElement()
        { }

        #region Parent

        [NonSerialized]
        private ILayoutContainer _parent = null;
        [NonSerialized]
        private ILayoutRoot _root = null;
        [XmlIgnore]
        public ILayoutContainer Parent
        {
            get { return _parent; }
            set
            {
                if (_parent != value)
                {
                    ILayoutContainer oldValue = _parent;
                    ILayoutRoot oldRoot = _root;
                    RaisePropertyChanging("Parent");
                    OnParentChanging(oldValue, value);
                    _parent = value;
                    OnParentChanged(oldValue, value);

                    _root = Root;
                    if (oldRoot != _root)
                        OnRootChanged(oldRoot, _root);

                    RaisePropertyChanged("Parent");

                    var root = Root as LayoutRoot;
                    if (root != null)
                        root.FireLayoutUpdated();
                }
            }
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle execute code before to the Parent property changes.
        /// </summary>
        protected virtual void OnParentChanging(ILayoutContainer oldValue, ILayoutContainer newValue)
        {
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Parent property.
        /// </summary>
        protected virtual void OnParentChanged(ILayoutContainer oldValue, ILayoutContainer newValue)
        {
            
        }


        protected virtual void OnRootChanged(ILayoutRoot oldRoot, ILayoutRoot newRoot)
        {
            if (oldRoot != null)
                ((LayoutRoot)oldRoot).OnLayoutElementRemoved(this);
            if (newRoot != null)
                ((LayoutRoot)newRoot).OnLayoutElementAdded(this);        
        }


        #endregion

        [field: NonSerialized]
        [field: XmlIgnore]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        [field: NonSerialized]
        [field: XmlIgnore]
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void RaisePropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
        }

        public ILayoutRoot Root
        {
            get
            {
                var parent = Parent;

                while (parent != null && (!(parent is ILayoutRoot)))
                {
                    parent = parent.Parent;
                }

                return parent as ILayoutRoot;
            }
        }


#if DEBUG
        public virtual void ConsoleDump(int tab)
        {
            System.Diagnostics.Debug.Write(new String(' ', tab * 4));
            System.Diagnostics.Debug.WriteLine(this.ToString());
        }
#endif
    }
}
