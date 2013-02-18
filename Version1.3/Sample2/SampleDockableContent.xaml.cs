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

namespace Sample2
{
    /// <summary>
    /// Interaction logic for SampleDockableContent.xaml
    /// </summary>
    public partial class SampleDockableContent : DockableContent
    {
        public SampleDockableContent()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public override void SaveLayout(System.Xml.XmlWriter storeWriter)
        {
            storeWriter.WriteAttributeString("TextSaved", txtTestFocus.Text);
            
            base.SaveLayout(storeWriter);
        }

        public override void RestoreLayout(System.Xml.XmlElement contentElement)
        {
            txtTestFocus.Text = contentElement.GetAttribute("TextSaved");
            
            base.RestoreLayout(contentElement);
        }
    }
}
