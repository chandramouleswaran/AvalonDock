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

namespace AvalonDock.Controls
{
    internal static class TransformExtentions
    {
        public static Point PointToScreenDPI(this Visual visual, Point pt)
        {
            Point resultPt = visual.PointToScreen(pt);
            return TransformToDeviceDPI(visual, resultPt);
        }

        public static Point PointToScreenDPIWithoutFlowDirection(this FrameworkElement element, Point point)
        {
            if (FrameworkElement.GetFlowDirection(element) == FlowDirection.RightToLeft)
            {
                var actualSize = element.TransformActualSizeToAncestor();
                Point leftToRightPoint = new Point(
                    actualSize.Width - point.X,
                    point.Y);
                return element.PointToScreenDPI(leftToRightPoint);
            }

            return element.PointToScreenDPI(point);
        }



        public static Rect GetScreenArea(this FrameworkElement element)
        {
        //    return new Rect(element.PointToScreenDPI(new Point()),
        //        element.TransformActualSizeToAncestor());
        //}

        //public static Rect GetScreenAreaWithoutFlowDirection(this FrameworkElement element)
        //{
            var point = element.PointToScreenDPI(new Point());
            if (FrameworkElement.GetFlowDirection(element) == FlowDirection.RightToLeft)
            {
                var actualSize = element.TransformActualSizeToAncestor();
                Point leftToRightPoint = new Point(
                    actualSize.Width - point.X,
                    point.Y);
                return new Rect(leftToRightPoint,
                    actualSize);
            }

            return new Rect(point,
                element.TransformActualSizeToAncestor());
        }

        public static Point TransformToDeviceDPI(this Visual visual, Point pt)
        {
            Matrix m = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return new Point(pt.X / m.M11, pt.Y / m.M22);
        }

        public static Size TransformFromDeviceDPI(this Visual visual, Size size)
        {
            Matrix m = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return new Size(size.Width * m.M11, size.Height * m.M22);
        }

        public static Point TransformFromDeviceDPI(this Visual visual, Point pt)
        {
            Matrix m = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return new Point(pt.X * m.M11, pt.Y * m.M22);
        }

        public static bool CanTransform(this Visual visual)
        {
            return PresentationSource.FromVisual(visual) != null;
        }

        public static Size TransformActualSizeToAncestor(this FrameworkElement element)
        {
            if (PresentationSource.FromVisual(element) == null)
                return new Size(element.ActualWidth, element.ActualHeight);

            var parentWindow = PresentationSource.FromVisual(element).RootVisual;
            var transformToWindow = element.TransformToAncestor(parentWindow);
            return transformToWindow.TransformBounds(new Rect(0, 0, element.ActualWidth, element.ActualHeight)).Size;
        }

        public static Size TransformSizeToAncestor(this FrameworkElement element, Size sizeToTransform)
        {
            if (PresentationSource.FromVisual(element) == null)
                return sizeToTransform;

            var parentWindow = PresentationSource.FromVisual(element).RootVisual;
            var transformToWindow = element.TransformToAncestor(parentWindow);
            return transformToWindow.TransformBounds(new Rect(0, 0, sizeToTransform.Width, sizeToTransform.Height)).Size;
        }

        public static GeneralTransform TansformToAncestor(this FrameworkElement element)
        {
            if (PresentationSource.FromVisual(element) == null)
                return new MatrixTransform(Matrix.Identity);

            var parentWindow = PresentationSource.FromVisual(element).RootVisual;
            return element.TransformToAncestor(parentWindow);
        }

    }
}
