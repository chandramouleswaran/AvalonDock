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
using AvalonDock.Layout;
using System.Windows.Threading;

namespace AvalonDock.Controls
{
    internal abstract class DropTarget<T> : DropTargetBase, IDropTarget where T : FrameworkElement
    {
        protected DropTarget(T targetElement, Rect detectionRect, DropTargetType type)
        {
            _targetElement = targetElement;
            _detectionRect = new Rect[] { detectionRect };
            _type = type;
        }
        
        protected DropTarget(T targetElement, IEnumerable<Rect> detectionRects, DropTargetType type)
        {
            _targetElement = targetElement;
            _detectionRect = detectionRects.ToArray();
            _type = type;
        }

        Rect[] _detectionRect;

        public Rect[] DetectionRects
        {
            get { return _detectionRect; }
        }


        T _targetElement;
        public T TargetElement
        {
            get { return _targetElement; }
        }

        DropTargetType _type;

        public DropTargetType Type
        {
            get { return _type; }
        }

        protected virtual void Drop(LayoutAnchorableFloatingWindow floatingWindow)
        { }

        protected virtual void Drop(LayoutDocumentFloatingWindow floatingWindow)
        { }


        public void Drop(LayoutFloatingWindow floatingWindow)
        {
            var root = floatingWindow.Root;
            var currentActiveContent = floatingWindow.Root.ActiveContent;
            var fwAsAnchorable = floatingWindow as LayoutAnchorableFloatingWindow;

            if (fwAsAnchorable != null)
            {
                this.Drop(fwAsAnchorable);
            }
            else
            {
                var fwAsDocument = floatingWindow as LayoutDocumentFloatingWindow;
                this.Drop(fwAsDocument);
            }

            Dispatcher.BeginInvoke(new Action(() =>
                {
                    currentActiveContent.IsSelected = false;
                    currentActiveContent.IsActive = false;
                    currentActiveContent.IsActive = true;
                }), DispatcherPriority.Background);
        }

        public virtual bool HitTest(Point dragPoint)
        {
            return _detectionRect.Any(dr => dr.Contains(dragPoint));
        }

        public abstract Geometry GetPreviewPath(OverlayWindow overlayWindow, LayoutFloatingWindow floatingWindow);



        public void DragEnter()
        {
            SetIsDraggingOver(TargetElement, true);
        }

        public void DragLeave()
        {
            SetIsDraggingOver(TargetElement, false);
        }
    }
}
