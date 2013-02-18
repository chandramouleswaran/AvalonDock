using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using AvalonDock;
using System.IO;

namespace AvalonDockTest
{
    /// <summary>
    /// Interaction logic for Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {
        public Window5()
        {
            InitializeComponent();
        }

        MemoryStream savedLayout = new MemoryStream();

        private void ShowDockingManager_Click(object sender, RoutedEventArgs e)
        {
            savedLayout.Position = 0;
            _dockingManager.RestoreLayout(savedLayout);
            _dockingManager.Hide(ShowMeFirst);
            savedLayout.Close();
        }


        private void _dockingManager_Loaded(object sender, RoutedEventArgs e)
        {
            _dockingManager.SaveLayout(savedLayout);

            _dockingManager.Hide(ShowMeSecond);
            _dockingManager.Hide(AlsoShowMeSecond);

        }
    }
}
