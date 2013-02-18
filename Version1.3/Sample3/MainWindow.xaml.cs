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

namespace Sample3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BuildDockingLayout();

            Application.Current.Resources["ThemeDictionary"] = new ResourceDictionary();
        }

        void BuildDockingLayout()
        {
            dockManager.Content = null;

            //TreeView dockable content
            var trv = new TreeView();
            trv.Items.Add(new TreeViewItem() { Header = "Item1" });
            trv.Items.Add(new TreeViewItem() { Header = "Item2" });
            trv.Items.Add(new TreeViewItem() { Header = "Item3" });
            trv.Items.Add(new TreeViewItem() { Header = "Item4" });
            ((TreeViewItem)trv.Items[0]).Items.Add(new TreeViewItem() { Header = "SubItem1" });
            ((TreeViewItem)trv.Items[0]).Items.Add(new TreeViewItem() { Header = "SubItem2" });
            ((TreeViewItem)trv.Items[1]).Items.Add(new TreeViewItem() { Header = "SubItem3" });
            ((TreeViewItem)trv.Items[2]).Items.Add(new TreeViewItem() { Header = "SubItem4" });
            var treeviewContent = new DockableContent() { Title = "Explorer", Content = trv };

            treeviewContent.Show(dockManager, AnchorStyle.Bottom);

            //TextBox invo dockable content
            var treeviewInfoContent = new DockableContent() { Title = "Explorer Info", Content = new TextBox() { Text = "Explorer Info Text", IsReadOnly = true } };
            treeviewContent.ContainerPane.Items.Add(treeviewInfoContent);


            //ListView dockable content
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn() { Header = "Date" });
            gridView.Columns.Add(new GridViewColumn() { Header = "Day Of Weeek", DisplayMemberBinding = new Binding("DayOfWeek") });
            gridView.Columns.Add(new GridViewColumn() { Header = "Year", DisplayMemberBinding = new Binding("Year") });
            gridView.Columns.Add(new GridViewColumn() { Header = "Month", DisplayMemberBinding = new Binding("Month") });
            gridView.Columns.Add(new GridViewColumn() { Header = "Second", DisplayMemberBinding = new Binding("Second") });
            var listView = new ListView() { View = gridView };
            listView.Items.Add(DateTime.Now);
            listView.Items.Add(DateTime.Now.AddYears(-1));
            listView.Items.Add(DateTime.Now.AddMonths(15));
            listView.Items.Add(DateTime.Now.AddHours(354));

            var listViewContent = new DockableContent() { Title = "Date & Times", Content = listView };
            listViewContent.ShowAsFloatingWindow(dockManager, true);

            //TextBox dockable content
            var textboxSampleContent = new DockableContent() { Title = "Date & Times Info", Content = new TextBox() { Text = "Date & Times Info Text", IsReadOnly = true } };
            listViewContent.ContainerPane.Items.Add(textboxSampleContent);


            //DataGrid document
            //var dataGrid = new DataGrid();
            //var rnd = new Random();
            //var data = new List<Tuple<double, double, double, double>>();
            //for (int i = 0; i < 100; i++)
            //{
            //    data.Add(Tuple.Create(rnd.NextDouble(), rnd.NextDouble() * 10.0, rnd.NextDouble() * 100.0, rnd.NextDouble() * 1000.0));
            //}

            //dataGrid.ItemsSource = data;

            //var dataGridDocument = new DocumentContent() { Title = "Data", IsLocked = true, Content = dataGrid };
            //dataGridDocument.Show(dockManager);

            ////DataGrid Info Text sample
            //var dataGridInfoContent = new DockableContent() { Title = "Data Info", Content = new TextBox() { Text = "Data Info Text" } };
            //dataGridInfoContent.ShowAsDocument(dockManager);

        }

        private void SetDefaultTheme(object sender, RoutedEventArgs e)
        {
            ThemeFactory.ResetTheme();
        }

        private void ChangeCustomTheme(object sender, RoutedEventArgs e)
        {
            string uri = (string)((MenuItem)sender).Tag;
            ThemeFactory.ChangeTheme(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        private void ChangeStandardTheme(object sender, RoutedEventArgs e)
        {
            string name = (string)((MenuItem)sender).Tag;
            ThemeFactory.ChangeTheme(name);
        }

        private void ChangeColor(object sender, RoutedEventArgs e)
        {
            
            ThemeFactory.ChangeColors((Color)ColorConverter.ConvertFromString(((MenuItem)sender).Header.ToString()));
        }
    }
}
