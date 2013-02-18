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

namespace AvalonDock.TestApp
{
    /// <summary>
    /// Interaction logic for TestUserControl.xaml
    /// </summary>
    public partial class TestUserControl : UserControl
    {
        public TestUserControl()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(TestUserControl_Loaded);
            this.Unloaded += new RoutedEventHandler(TestUserControl_Unloaded);
        }

        void TestUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        void TestUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
