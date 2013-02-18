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
using System.Windows.Media;
using System.Diagnostics;
using AvalonDock.Layout;

namespace AvalonDock.Controls
{
    internal class DocumentPaneGroupDropTarget : DropTarget<LayoutDocumentPaneGroupControl>
    {
        internal DocumentPaneGroupDropTarget(LayoutDocumentPaneGroupControl paneControl, Rect detectionRect, DropTargetType type)
            : base(paneControl, detectionRect, type)
        {
            _targetPane = paneControl;
        }

        LayoutDocumentPaneGroupControl _targetPane;

        protected override void Drop(LayoutDocumentFloatingWindow floatingWindow)
        {
            ILayoutPane targetModel = _targetPane.Model as ILayoutPane;
            
            switch (Type)
            {
                case DropTargetType.DocumentPaneGroupDockInside:
                    #region DropTargetType.DocumentPaneGroupDockInside
                    {
                        var paneGroupModel = targetModel as LayoutDocumentPaneGroup;
                        var paneModel = paneGroupModel.Children[0] as LayoutDocumentPane;
                        var sourceModel = floatingWindow.RootDocument;

                        paneModel.Children.Insert(0, sourceModel);
                    }
                    break;
                    #endregion
            }
            base.Drop(floatingWindow);
        }

        protected override void Drop(LayoutAnchorableFloatingWindow floatingWindow)
        {
            ILayoutPane targetModel = _targetPane.Model as ILayoutPane;

            switch (Type)
            {
                case DropTargetType.DocumentPaneGroupDockInside:
                    #region DropTargetType.DocumentPaneGroupDockInside
                    {
                        var paneGroupModel = targetModel as LayoutDocumentPaneGroup;
                        var paneModel = paneGroupModel.Children[0] as LayoutDocumentPane;
                        var layoutAnchorablePaneGroup = floatingWindow.RootPanel as LayoutAnchorablePaneGroup;

                        int i = 0;
                        foreach (var anchorableToImport in layoutAnchorablePaneGroup.Descendents().OfType<LayoutAnchorable>().ToArray())
                        {
                            paneModel.Children.Insert(i, anchorableToImport);
                            i++;
                        }
                    }
                    break;
                    #endregion
            }

            base.Drop(floatingWindow);
        }

        public override System.Windows.Media.Geometry GetPreviewPath(OverlayWindow overlayWindow, LayoutFloatingWindow floatingWindowModel)
        {
            switch (Type)
            {
                case DropTargetType.DocumentPaneGroupDockInside:
                    #region DropTargetType.DocumentPaneGroupDockInside
                    {
                        var targetScreenRect = TargetElement.GetScreenArea();
                        targetScreenRect.Offset(-overlayWindow.Left, -overlayWindow.Top);

                        return new RectangleGeometry(targetScreenRect);
                    } 
                    #endregion
            }

            return null;
        }

    }
}
