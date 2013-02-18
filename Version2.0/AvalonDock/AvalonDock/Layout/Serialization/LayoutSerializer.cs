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
using System.IO;
using AvalonDock.Controls;
using System.Windows;

namespace AvalonDock.Layout.Serialization
{
    public abstract class LayoutSerializer
    {
        DockingManager _manager;

        public LayoutSerializer(DockingManager manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");

            _manager = manager;
            _previousAnchorables = _manager.Layout.Descendents().OfType<LayoutAnchorable>().ToArray();
            _previousDocuments = _manager.Layout.Descendents().OfType<LayoutDocument>().ToArray();
        }

        LayoutAnchorable[] _previousAnchorables = null;
        LayoutDocument[] _previousDocuments = null;

        public DockingManager Manager
        {
            get { return _manager; }
        }

        public event EventHandler<LayoutSerializationCallbackEventArgs> LayoutSerializationCallback;

        protected virtual void FixupLayout(LayoutRoot layout)
        {
            //fix container panes
            foreach (var lcToAttach in layout.Descendents().OfType<ILayoutPreviousContainer>().Where(lc => lc.PreviousContainerId != null))
            {
                var paneContainerToAttach = layout.Descendents().OfType<ILayoutPaneSerializable>().FirstOrDefault(lps => lps.Id == lcToAttach.PreviousContainerId);
                if (paneContainerToAttach == null)
                    throw new ArgumentException(string.Format("Unable to find a pane with id ='{0}'", lcToAttach.PreviousContainerId));

                lcToAttach.PreviousContainer = paneContainerToAttach as ILayoutContainer;
            }


            //now fix the content of the layoutcontents
            foreach (var lcToFix in layout.Descendents().OfType<LayoutAnchorable>().Where(lc => lc.Content == null).ToArray())
            {
                LayoutAnchorable previousAchorable = null;
                if (lcToFix.ContentId != null)
                { 
                    //try find the content in replaced layout
                    previousAchorable = _previousAnchorables.FirstOrDefault(a => a.ContentId == lcToFix.ContentId);
                }

                if (LayoutSerializationCallback != null)
                {
                    var args = new LayoutSerializationCallbackEventArgs(lcToFix, previousAchorable != null ? previousAchorable.Content : null);
                    LayoutSerializationCallback(this, args);
                    if (args.Cancel)
                        lcToFix.Close();
                    else if (args.Content != null)
                        lcToFix.Content = args.Content;
                    else if (args.Model.Content != null)
                        lcToFix.Hide(false);
                }
                else if (previousAchorable == null)
                    lcToFix.Hide(false);
                else
                {
                    lcToFix.Content = previousAchorable.Content;
                    lcToFix.IconSource = previousAchorable.IconSource;
                }
            }


            foreach (var lcToFix in layout.Descendents().OfType<LayoutDocument>().Where(lc => lc.Content == null).ToArray())
            {
                LayoutDocument previousDocument = null;
                if (lcToFix.ContentId != null)
                {
                    //try find the content in replaced layout
                    previousDocument = _previousDocuments.FirstOrDefault(a => a.ContentId == lcToFix.ContentId);
                }

                if (LayoutSerializationCallback != null)
                {
                    var args = new LayoutSerializationCallbackEventArgs(lcToFix, previousDocument != null ? previousDocument.Content : null);
                    LayoutSerializationCallback(this, args);

                    if (args.Cancel)
                        lcToFix.Close();
                    else if (args.Content != null)
                        lcToFix.Content = args.Content;
                    else if (args.Model.Content != null)
                        lcToFix.Close();
                }
                else if (previousDocument == null)
                    lcToFix.Close();
                else
                    lcToFix.Content = previousDocument.Content;
            }


            layout.CollectGarbage();
        }

        protected void StartDeserialization()
        {
            Manager.SuspendDocumentsSourceBinding = true;
            Manager.SuspendAnchorablesSourceBinding = true;
        }

        protected void EndDeserialization()
        {
            Manager.SuspendDocumentsSourceBinding = false;
            Manager.SuspendAnchorablesSourceBinding = false;
        }
    }
}
