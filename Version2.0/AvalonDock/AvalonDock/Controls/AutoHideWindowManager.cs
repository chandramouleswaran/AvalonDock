using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using AvalonDock.Layout;

namespace AvalonDock.Controls
{
    class AutoHideWindowManager
    {
        DockingManager _manager;

        internal AutoHideWindowManager(DockingManager manager)
        {
            _manager = manager;
            SetupCloseTimer();
        }


        WeakReference _currentAutohiddenAnchor = null;
       
        public void ShowAutoHideWindow(LayoutAnchorControl anchor)
        {
            StopCloseTimer();
            _currentAutohiddenAnchor = new WeakReference(anchor);
            _manager.AutoHideWindow.Show(anchor);
            StartCloseTimer();
        }

        public void HideAutoWindow(LayoutAnchorControl anchor = null)
        {
            if (anchor == null ||
                anchor == _currentAutohiddenAnchor.GetValueOrDefault<LayoutAnchorControl>())
            {
                StopCloseTimer();
            }
            else
                System.Diagnostics.Debug.Assert(false);
        }

        DispatcherTimer _closeTimer = null;
        void SetupCloseTimer()
        {
            _closeTimer = new DispatcherTimer(DispatcherPriority.Background);
            _closeTimer.Interval = TimeSpan.FromMilliseconds(1500);
            _closeTimer.Tick += (s, e) =>
            {
                if (_manager.AutoHideWindow.IsWin32MouseOver ||
                    ((LayoutAnchorable)_manager.AutoHideWindow.Model).IsActive ||
                    _manager.AutoHideWindow.IsResizing)
                    return;

                StopCloseTimer();
            };
        }

        void StartCloseTimer()
        { 
            _closeTimer.Start();
        
        }

        void StopCloseTimer()
        {
            _closeTimer.Stop();
            _manager.AutoHideWindow.Hide();
        }
    }
}
