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
using AvalonDock.Layout;
using System.Windows.Input;
using System.Windows;
using AvalonDock.Commands;
using System.Windows.Data;

namespace AvalonDock.Controls
{
    public class LayoutAnchorableItem : LayoutItem
    {
        LayoutAnchorable _anchorable;
        internal LayoutAnchorableItem()
        {

        }

        internal override void Attach(LayoutContent model)
        {
            _anchorable = model as LayoutAnchorable;
            _anchorable.IsVisibleChanged += new EventHandler(_anchorable_IsVisibleChanged);

            base.Attach(model);
        }

        internal override void Detach()
        {
            _anchorable.IsVisibleChanged -= new EventHandler(_anchorable_IsVisibleChanged);
            _anchorable = null;
            base.Detach();
        }

        protected override void Close()
        {
            var dockingManager = _anchorable.Root.Manager;
            dockingManager._ExecuteCloseCommand(_anchorable);
        }

        ICommand _defaultHideCommand;
        ICommand _defaultAutoHideCommand;
        ICommand _defaultDockCommand;

        protected override void InitDefaultCommands()
        {
            _defaultHideCommand = new RelayCommand((p) => ExecuteHideCommand(p), (p) => CanExecuteHideCommand(p));
            _defaultAutoHideCommand = new RelayCommand((p) => ExecuteAutoHideCommand(p), (p) => CanExecuteAutoHideCommand(p));
            _defaultDockCommand = new RelayCommand((p) => ExecuteDockCommand(p), (p) => CanExecuteDockCommand(p));

            base.InitDefaultCommands();
        }

        protected override void ClearDefaultBindings()
        {
            if (HideCommand == _defaultHideCommand)
                BindingOperations.ClearBinding(this, HideCommandProperty);
            if (AutoHideCommand == _defaultAutoHideCommand)
                BindingOperations.ClearBinding(this, AutoHideCommandProperty);
            if (DockCommand == _defaultDockCommand)
                BindingOperations.ClearBinding(this, DockCommandProperty);

            base.ClearDefaultBindings();
        }

        protected override void SetDefaultBindings()
        {
            if (HideCommand == null)
                HideCommand = _defaultHideCommand;
            if (AutoHideCommand == null)
                AutoHideCommand = _defaultAutoHideCommand;
            if (DockCommand == null)
                DockCommand = _defaultDockCommand;

            Visibility = _anchorable.IsVisible ? Visibility.Visible : System.Windows.Visibility.Hidden;
            base.SetDefaultBindings();
        }


        #region HideCommand

        /// <summary>
        /// HideCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty HideCommandProperty =
            DependencyProperty.Register("HideCommand", typeof(ICommand), typeof(LayoutAnchorableItem),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnHideCommandChanged),
                    new CoerceValueCallback(CoerceHideCommandValue)));

        /// <summary>
        /// Gets or sets the HideCommand property.  This dependency property 
        /// indicates the command to execute when an anchorable is hidden.
        /// </summary>
        public ICommand HideCommand
        {
            get { return (ICommand)GetValue(HideCommandProperty); }
            set { SetValue(HideCommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HideCommand property.
        /// </summary>
        private static void OnHideCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LayoutAnchorableItem)d).OnHideCommandChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the HideCommand property.
        /// </summary>
        protected virtual void OnHideCommandChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Coerces the HideCommand value.
        /// </summary>
        private static object CoerceHideCommandValue(DependencyObject d, object value)
        {
            return value;
        }


        private bool CanExecuteHideCommand(object parameter)
        {
            if (LayoutElement == null)
                return false;
            return _anchorable.CanHide;
        }

        private void ExecuteHideCommand(object parameter)
        {
            if (_anchorable != null && _anchorable.Root != null && _anchorable.Root.Manager != null)
                _anchorable.Root.Manager._ExecuteHideCommand(_anchorable);
        }

        #endregion

        #region AutoHideCommand

        /// <summary>
        /// AutoHideCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty AutoHideCommandProperty =
            DependencyProperty.Register("AutoHideCommand", typeof(ICommand), typeof(LayoutAnchorableItem),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnAutoHideCommandChanged),
                    new CoerceValueCallback(CoerceAutoHideCommandValue)));

        /// <summary>
        /// Gets or sets the AutoHideCommand property.  This dependency property 
        /// indicates the command to execute when user click the auto hide button.
        /// </summary>
        /// <remarks>By default this command toggles auto hide state for an anchorable.</remarks>
        public ICommand AutoHideCommand
        {
            get { return (ICommand)GetValue(AutoHideCommandProperty); }
            set { SetValue(AutoHideCommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the AutoHideCommand property.
        /// </summary>
        private static void OnAutoHideCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LayoutAnchorableItem)d).OnAutoHideCommandChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the AutoHideCommand property.
        /// </summary>
        protected virtual void OnAutoHideCommandChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Coerces the AutoHideCommand value.
        /// </summary>
        private static object CoerceAutoHideCommandValue(DependencyObject d, object value)
        {
            return value;
        }

        private bool CanExecuteAutoHideCommand(object parameter)
        {
            if (LayoutElement == null)
                return false;

            if (LayoutElement.FindParent<LayoutAnchorableFloatingWindow>() != null)
                return false;//is floating

            return _anchorable.CanAutoHide;
        }

        private void ExecuteAutoHideCommand(object parameter)
        {
            if (_anchorable != null && _anchorable.Root != null && _anchorable.Root.Manager != null)
                _anchorable.Root.Manager._ExecuteAutoHideCommand(_anchorable);
        }

        #endregion

        #region DockCommand

        /// <summary>
        /// DockCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty DockCommandProperty =
            DependencyProperty.Register("DockCommand", typeof(ICommand), typeof(LayoutAnchorableItem),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnDockCommandChanged),
                    new CoerceValueCallback(CoerceDockCommandValue)));

        /// <summary>
        /// Gets or sets the DockCommand property.  This dependency property 
        /// indicates the command to execute when user click the Dock button.
        /// </summary>
        /// <remarks>By default this command moves the anchorable inside the container pane which previously hosted the object.</remarks>
        public ICommand DockCommand
        {
            get { return (ICommand)GetValue(DockCommandProperty); }
            set { SetValue(DockCommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DockCommand property.
        /// </summary>
        private static void OnDockCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LayoutAnchorableItem)d).OnDockCommandChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DockCommand property.
        /// </summary>
        protected virtual void OnDockCommandChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Coerces the DockCommand value.
        /// </summary>
        private static object CoerceDockCommandValue(DependencyObject d, object value)
        {
            return value;
        }

        private bool CanExecuteDockCommand(object parameter)
        {
            if (LayoutElement == null)
                return false;
            return LayoutElement.FindParent<LayoutAnchorableFloatingWindow>() != null;
        }

        private void ExecuteDockCommand(object parameter)
        {
            LayoutElement.Root.Manager._ExecuteDockCommand(_anchorable);
        }

        #endregion

        #region Visibility
        ReentrantFlag _visibilityReentrantFlag = new ReentrantFlag();

        protected override void OnVisibilityChanged()
        {
            if (_anchorable != null && _anchorable.Root != null)
            {
                if (_visibilityReentrantFlag.CanEnter)
                {
                    using (_visibilityReentrantFlag.Enter())
                    {
                        if (Visibility == System.Windows.Visibility.Hidden)
                            _anchorable.Hide(false);
                        else if (Visibility == System.Windows.Visibility.Visible)
                            _anchorable.Show();
                    }
                }
            }

            base.OnVisibilityChanged();
        }


        void _anchorable_IsVisibleChanged(object sender, EventArgs e)
        {
            if (_anchorable != null && _anchorable.Root != null)
            {
                if (_visibilityReentrantFlag.CanEnter)
                {
                    using (_visibilityReentrantFlag.Enter())
                    {
                        if (_anchorable.IsVisible)
                            Visibility = Visibility.Visible;
                        else
                            Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        #endregion

        #region CanHide

        /// <summary>
        /// CanHide Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanHideProperty =
            DependencyProperty.Register("CanHide", typeof(bool), typeof(LayoutAnchorableItem),
                new FrameworkPropertyMetadata((bool)true,
                    new PropertyChangedCallback(OnCanHideChanged)));

        /// <summary>
        /// Gets or sets the CanHide property.  This dependency property 
        /// indicates if user can hide the anchorable item.
        /// </summary>
        public bool CanHide
        {
            get { return (bool)GetValue(CanHideProperty); }
            set { SetValue(CanHideProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanHide property.
        /// </summary>
        private static void OnCanHideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LayoutAnchorableItem)d).OnCanHideChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CanHide property.
        /// </summary>
        protected virtual void OnCanHideChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_anchorable != null)
                _anchorable.CanHide = (bool)e.NewValue;
        }

        #endregion


    }
}
