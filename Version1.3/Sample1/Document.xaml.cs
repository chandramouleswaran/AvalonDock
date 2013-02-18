using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AvalonDock;

namespace Sample1
{
    /// <summary>
    /// Interaction logic for Document.xaml
    /// </summary>
    public partial class Document : DocumentContent
    {
        public Document()
        {
            InitializeComponent();

            DataContext = this;
        }

        #region TextContent

        /// <summary>
        /// TextContent Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextContentProperty =
            DependencyProperty.Register("TextContent", typeof(string), typeof(Document),
                new FrameworkPropertyMetadata((string)string.Empty,
                    new PropertyChangedCallback(OnTextContentChanged)));

        /// <summary>
        /// Gets or sets the TextContent property.  This dependency property 
        /// indicates document text.
        /// </summary>
        public string TextContent
        {
            get { return (string)GetValue(TextContentProperty); }
            set { SetValue(TextContentProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TextContent property.
        /// </summary>
        private static void OnTextContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Document)d).OnTextContentChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TextContent property.
        /// </summary>
        protected virtual void OnTextContentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (TextContentChanged != null)
                TextContentChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// event raised when text changes
        /// </summary>
        public event EventHandler TextContentChanged;
        #endregion






    }
}
