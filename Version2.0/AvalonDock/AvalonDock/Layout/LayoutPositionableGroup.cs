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
using System.Globalization;

namespace AvalonDock.Layout
{
    [Serializable]
    public abstract class LayoutPositionableGroup<T> : LayoutGroup<T>, ILayoutPositionableElement, ILayoutPositionableElementWithActualSize where T : class, ILayoutElement
    {
        public LayoutPositionableGroup()
        { }

        GridLength _dockWidth = new GridLength(1.0, GridUnitType.Star);
        public GridLength DockWidth
        {
            get
            {
                return _dockWidth;
            }
            set
            {
                if (DockWidth != value)
                {
                    RaisePropertyChanging("DockWidth");
                    _dockWidth = value;
                    RaisePropertyChanged("DockWidth");

                    OnDockWidthChanged();
                }
            }
        }


        protected virtual void OnDockWidthChanged()
        {
        
        }

        GridLength _dockHeight = new GridLength(1.0, GridUnitType.Star);
        public GridLength DockHeight
        {
            get
            {
                return _dockHeight;
            }
            set
            {
                if (DockHeight != value)
                {
                    RaisePropertyChanging("DockHeight");
                    _dockHeight = value;
                    RaisePropertyChanged("DockHeight");

                    OnDockHeightChanged();
                }
            }
        }

        protected virtual void OnDockHeightChanged()
        { 
            
        }


        #region DockMinWidth

        private double _dockMinWidth = 25.0;
        public double DockMinWidth
        {
            get { return _dockMinWidth; }
            set
            {
                if (_dockMinWidth != value)
                {
                    MathHelper.AssertIsPositiveOrZero(value);
                    RaisePropertyChanging("DockMinWidth");
                    _dockMinWidth = value;
                    RaisePropertyChanged("DockMinWidth");
                }
            }
        }

        #endregion

        #region DockMinHeight

        private double _dockMinHeight = 25.0;
        public double DockMinHeight
        {
            get { return _dockMinHeight; }
            set
            {
                if (_dockMinHeight != value)
                {
                    MathHelper.AssertIsPositiveOrZero(value);
                    RaisePropertyChanging("DockMinHeight");
                    _dockMinHeight = value;
                    RaisePropertyChanged("DockMinHeight");
                }
            }
        }

        #endregion

        #region FloatingWidth

        private double _floatingWidth = 0.0;
        public double FloatingWidth
        {
            get { return _floatingWidth; }
            set
            {
                if (_floatingWidth != value)
                {
                    RaisePropertyChanging("FloatingWidth");
                    _floatingWidth = value;
                    RaisePropertyChanged("FloatingWidth");
                }
            }
        }

        #endregion

        #region FloatingHeight

        private double _floatingHeight = 0.0;
        public double FloatingHeight
        {
            get { return _floatingHeight; }
            set
            {
                if (_floatingHeight != value)
                {
                    RaisePropertyChanging("FloatingHeight");
                    _floatingHeight = value;
                    RaisePropertyChanged("FloatingHeight");
                }
            }
        }

        #endregion

        #region FloatingLeft

        private double _floatingLeft = 0.0;
        public double FloatingLeft
        {
            get { return _floatingLeft; }
            set
            {
                if (_floatingLeft != value)
                {
                    RaisePropertyChanging("FloatingLeft");
                    _floatingLeft = value;
                    RaisePropertyChanged("FloatingLeft");
                }
            }
        }

        #endregion

        #region FloatingTop

        private double _floatingTop = 0.0;
        public double FloatingTop
        {
            get { return _floatingTop; }
            set
            {
                if (_floatingTop != value)
                {
                    RaisePropertyChanging("FloatingTop");
                    _floatingTop = value;
                    RaisePropertyChanged("FloatingTop");
                }
            }
        }

        #endregion

        #region IsMaximized

        private bool _isMaximized = false;
        public bool IsMaximized
        {
            get { return _isMaximized; }
            set
            {
                if (_isMaximized != value)
                {
                    _isMaximized = value;
                    RaisePropertyChanged("IsMaximized");
                }
            }
        }

        #endregion


        [NonSerialized]
        double _actualWidth;
        double ILayoutPositionableElementWithActualSize.ActualWidth
        {
            get
            {
                return _actualWidth;
            }
            set
            {
                _actualWidth = value;
            }
        }
        
        [NonSerialized]
        double _actualHeight;
        double ILayoutPositionableElementWithActualSize.ActualHeight
        {
            get
            {
                return _actualHeight;
            }
            set
            {
                _actualHeight = value;
            }
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            if (DockWidth.Value != 1.0 || !DockWidth.IsStar)
                writer.WriteAttributeString("DockWidth", _gridLengthConverter.ConvertToInvariantString(DockWidth));
            if (DockHeight.Value != 1.0 || !DockHeight.IsStar)
                writer.WriteAttributeString("DockHeight", _gridLengthConverter.ConvertToInvariantString(DockHeight));

            if (DockMinWidth != 25.0)
                writer.WriteAttributeString("DocMinWidth", DockMinWidth.ToString(CultureInfo.InvariantCulture));
            if (DockMinHeight != 25.0)
                writer.WriteAttributeString("DockMinHeight", DockMinHeight.ToString(CultureInfo.InvariantCulture));

            if (FloatingWidth != 0.0)
                writer.WriteAttributeString("FloatingWidth", FloatingWidth.ToString(CultureInfo.InvariantCulture));
            if (FloatingHeight != 0.0)
                writer.WriteAttributeString("FloatingHeight", FloatingHeight.ToString(CultureInfo.InvariantCulture));
            if (FloatingLeft != 0.0)
                writer.WriteAttributeString("FloatingLeft", FloatingLeft.ToString(CultureInfo.InvariantCulture));
            if (FloatingTop != 0.0)
                writer.WriteAttributeString("FloatingTop", FloatingTop.ToString(CultureInfo.InvariantCulture));
            
            base.WriteXml(writer);
        }

        static GridLengthConverter _gridLengthConverter = new GridLengthConverter();
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.MoveToAttribute("DockWidth"))
                _dockWidth = (GridLength)_gridLengthConverter.ConvertFromInvariantString(reader.Value);
            if (reader.MoveToAttribute("DockHeight"))
                _dockHeight = (GridLength)_gridLengthConverter.ConvertFromInvariantString(reader.Value);

            if (reader.MoveToAttribute("DocMinWidth"))
                _dockMinWidth = double.Parse(reader.Value, CultureInfo.InvariantCulture);
            if (reader.MoveToAttribute("DocMinHeight"))
                _dockMinHeight = double.Parse(reader.Value, CultureInfo.InvariantCulture);

            if (reader.MoveToAttribute("FloatingWidth"))
                _floatingWidth = double.Parse(reader.Value, CultureInfo.InvariantCulture);
            if (reader.MoveToAttribute("FloatingHeight"))
                _floatingHeight = double.Parse(reader.Value, CultureInfo.InvariantCulture);
            if (reader.MoveToAttribute("FloatingLeft"))
                _floatingLeft = double.Parse(reader.Value, CultureInfo.InvariantCulture);
            if (reader.MoveToAttribute("FloatingTop"))
                _floatingTop = double.Parse(reader.Value, CultureInfo.InvariantCulture);

            base.ReadXml(reader);
        }
    }
}
