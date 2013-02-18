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

namespace AvalonDock.DemoApp
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Click!");
        }
    }
}
