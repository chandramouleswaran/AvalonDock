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
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using AvalonDock.Layout;

namespace AvalonDock.Controls
{
    class DragService
    {
        DockingManager _manager;
        LayoutFloatingWindowControl _floatingWindow;

        public DragService(LayoutFloatingWindowControl floatingWindow)
        {
            _floatingWindow = floatingWindow;
            _manager = floatingWindow.Model.Root.Manager;


            GetOverlayWindowHosts();
        }

        List<IOverlayWindowHost> _overlayWindowHosts = new List<IOverlayWindowHost>();
        void GetOverlayWindowHosts()
        {
            _overlayWindowHosts.AddRange(_manager.GetFloatingWindowsByZOrder().OfType<LayoutAnchorableFloatingWindowControl>().Where(fw => fw != _floatingWindow && fw.IsVisible));
            _overlayWindowHosts.Add(_manager);
        }

        IOverlayWindowHost _currentHost;
        IOverlayWindow _currentWindow;
        List<IDropArea> _currentWindowAreas = new List<IDropArea>();
        IDropTarget _currentDropTarget;

        public void UpdateMouseLocation(Point dragPosition)
        {
            var floatingWindowModel = _floatingWindow.Model as LayoutFloatingWindow;

            var newHost = _overlayWindowHosts.FirstOrDefault(oh => oh.HitTest(dragPosition));

            if (_currentHost != null || _currentHost != newHost)
            { 
                //is mouse still inside current overlay window host?
                if ((_currentHost != null && !_currentHost.HitTest(dragPosition)) ||
                    _currentHost != newHost)
                {
                    //esit drop target
                    if (_currentDropTarget != null)
                        _currentWindow.DragLeave(_currentDropTarget);
                    _currentDropTarget = null;

                    //exit area
                    _currentWindowAreas.ForEach(a =>
                        _currentWindow.DragLeave(a));
                    _currentWindowAreas.Clear();

                    //hide current overlay window
                    if (_currentWindow != null)
                        _currentWindow.DragLeave(_floatingWindow);
                    if (_currentHost != null)
                        _currentHost.HideOverlayWindow();
                    _currentHost = null;
                }

                if (_currentHost != newHost)
                {
                    _currentHost = newHost;
                    _currentWindow = _currentHost.ShowOverlayWindow(_floatingWindow);
                    _currentWindow.DragEnter(_floatingWindow);
                }
            }

            if (_currentHost == null)
                return;

            if (_currentDropTarget != null &&
                !_currentDropTarget.HitTest(dragPosition))
            {
                _currentWindow.DragLeave(_currentDropTarget);
                _currentDropTarget = null;
            }

            List<IDropArea> areasToRemove = new List<IDropArea>();
            _currentWindowAreas.ForEach(a =>
            { 
                //is mouse still inside this area?
                if (!a.DetectionRect.Contains(dragPosition))
                {
                    _currentWindow.DragLeave(a);
                    areasToRemove.Add(a);
                }
            });

            areasToRemove.ForEach(a =>
                _currentWindowAreas.Remove(a));


            var areasToAdd = 
                _currentHost.GetDropAreas(_floatingWindow).Where(cw => !_currentWindowAreas.Contains(cw) && cw.DetectionRect.Contains(dragPosition)).ToList();

            _currentWindowAreas.AddRange(areasToAdd);

            areasToAdd.ForEach(a =>
                _currentWindow.DragEnter(a));

            if (_currentDropTarget == null)
            {
                _currentWindowAreas.ForEach(wa =>
                    {
                        if (_currentDropTarget != null)
                            return;

                        _currentDropTarget = _currentWindow.GetTargets().FirstOrDefault(dt => dt.HitTest(dragPosition));
                        if (_currentDropTarget != null)
                        {
                            _currentWindow.DragEnter(_currentDropTarget);
                            return;
                        }
                    });
            }

        }

        public void Drop(Point dropLocation, out bool dropHandled)
        { 
            dropHandled = false;

            UpdateMouseLocation(dropLocation);

            var floatingWindowModel = _floatingWindow.Model as LayoutFloatingWindow;
            var root = floatingWindowModel.Root;

            if (_currentHost != null)
                _currentHost.HideOverlayWindow();

            if (_currentDropTarget != null)
            {
                _currentWindow.DragDrop(_currentDropTarget);
                root.CollectGarbage();
                dropHandled = true;
            }

            
            _currentWindowAreas.ForEach(a => _currentWindow.DragLeave(a));
            
            if (_currentDropTarget != null)
                _currentWindow.DragLeave(_currentDropTarget);
            if (_currentWindow != null)
                _currentWindow.DragLeave(_floatingWindow);
            _currentWindow = null;

            _currentHost = null;
        }

        internal void Abort()
        {
            var floatingWindowModel = _floatingWindow.Model as LayoutFloatingWindow;

            _currentWindowAreas.ForEach(a => _currentWindow.DragLeave(a));

            if (_currentDropTarget != null)
                _currentWindow.DragLeave(_currentDropTarget);
            if (_currentWindow != null)
                _currentWindow.DragLeave(_floatingWindow);
            _currentWindow = null;
            if (_currentHost != null)
                _currentHost.HideOverlayWindow();
            _currentHost = null;
        }
    }
}
