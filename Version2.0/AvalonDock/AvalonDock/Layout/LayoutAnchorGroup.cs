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
using System.Xml.Serialization;

namespace AvalonDock.Layout
{
    [ContentProperty("Children")]
    [Serializable]
    public class LayoutAnchorGroup : LayoutGroup<LayoutAnchorable>, ILayoutPreviousContainer, ILayoutPaneSerializable
    {
        public LayoutAnchorGroup()
        {
        }

        protected override bool GetVisibility()
        {
            return Children.Count > 0;
        }


        #region PreviousContainer

        [field:NonSerialized]
        private ILayoutContainer _previousContainer = null;
        [XmlIgnore]
        ILayoutContainer ILayoutPreviousContainer.PreviousContainer
        {
            get { return _previousContainer; }
            set
            {
                if (_previousContainer != value)
                {
                    _previousContainer = value;
                    RaisePropertyChanged("PreviousContainer");
                    var paneSerializable = _previousContainer as ILayoutPaneSerializable;
                    if (paneSerializable != null &&
                        paneSerializable.Id == null)
                        paneSerializable.Id = Guid.NewGuid().ToString();
                }
            }
        }

        #endregion

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

        string ILayoutPreviousContainer.PreviousContainerId
        {
            get;
            set;
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            if (_id != null)
                writer.WriteAttributeString("Id", _id);
            if (_previousContainer != null)
            {
                var paneSerializable = _previousContainer as ILayoutPaneSerializable;
                if (paneSerializable != null)
                {
                    writer.WriteAttributeString("PreviousContainerId", paneSerializable.Id);
                }
            }

            base.WriteXml(writer);
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.MoveToAttribute("Id"))
                _id = reader.Value;
            if (reader.MoveToAttribute("PreviousContainerId"))
                ((ILayoutPreviousContainer)this).PreviousContainerId = reader.Value;


            base.ReadXml(reader);
        }
    }
}
