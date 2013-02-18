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

namespace AvalonDock.Layout
{
    [ContentProperty("RootDocument")]
    [Serializable]
    public class LayoutDocumentFloatingWindow : LayoutFloatingWindow
    {
        public LayoutDocumentFloatingWindow()
        {

        }

        #region RootDocument

        private LayoutDocument _rootDocument = null;
        public LayoutDocument RootDocument
        {
            get { return _rootDocument; }
            set
            {
                if (_rootDocument != value)
                {
                    RaisePropertyChanging("RootDocument");
                    _rootDocument = value;
                    if (_rootDocument != null)
                        _rootDocument.Parent = this;
                    RaisePropertyChanged("RootDocument");

                    if (RootDocumentChanged != null)
                        RootDocumentChanged(this, EventArgs.Empty);
                }
            }
        }


        public event EventHandler RootDocumentChanged;

        #endregion

        public override IEnumerable<ILayoutElement> Children
        {
            get
            {
                if (RootDocument == null)
                    yield break;

                yield return RootDocument;
            }
        }

        public override void RemoveChild(ILayoutElement element)
        {
            Debug.Assert(element == RootDocument && element != null);
            RootDocument = null;
        }

        public override void ReplaceChild(ILayoutElement oldElement, ILayoutElement newElement)
        {
            Debug.Assert(oldElement == RootDocument && oldElement != null);
            RootDocument = newElement as LayoutDocument;
        }

        public override int ChildrenCount
        {
            get { return RootDocument != null ? 1 : 0; }
        }

        public override bool IsValid
        {
            get { return RootDocument != null; }
        }


#if DEBUG
        public override void ConsoleDump(int tab)
        {
            System.Diagnostics.Debug.Write(new string(' ', tab * 4));
            System.Diagnostics.Debug.WriteLine("FloatingDocumentWindow()");

            RootDocument.ConsoleDump(tab + 1);
        }
#endif
    }

}
