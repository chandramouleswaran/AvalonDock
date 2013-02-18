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
using System.IO;
using System.ComponentModel;

namespace Sample2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = this;
            
        }

        public DockableContent[] HiddenContents
        {
            get { return DockManager.DockableContents.Where(dc => dc.State == DockableContentState.Hidden).ToArray(); }
        }

        #region Load/Unload Initial layout
        private void DockManager_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("layout.xml"))
                DockManager.RestoreLayout(@"layout.xml");            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DockManager.SaveLayout(@"layout.xml");
        }
        #endregion

        private void OnShowDockableContent(object sender, RoutedEventArgs e)
        {
            var selectedContent = ((MenuItem)e.OriginalSource).DataContext as DockableContent;

            if (selectedContent.State != DockableContentState.Docked)
            {
                //show content as docked content
                selectedContent.Show(DockManager, AnchorStyle.Right);
            }

            selectedContent.Activate();
        }

        private void OnCreateNewDockableContent(object sender, RoutedEventArgs e)
        {
            string title = "NewContent";
            DockableContent[] cnts = this.DockManager.DockableContents.ToArray();
            int i = 1;
            while (cnts.FirstOrDefault(c => c.Title == title) != null)
            {
                title = string.Format("NewContent{0}", i);
                i++;
            }

            var newContent = new SampleDockableContent() { Name = title, Title = title };
            newContent.StateChanged += (s, args) =>
            { 
                if (PropertyChanged != null) 
                    PropertyChanged(this, new PropertyChangedEventArgs("HiddenContents")); 

                var dockContent = s as DockableContent;
                Log(string.Format("Content {0} changed state to {1}", dockContent.Title, dockContent.State));
            };

            newContent.Show(DockManager, AnchorStyle.Right);
            newContent.Activate();
        }

        private void ExportLayoutToDocument(object sender, RoutedEventArgs e)
        {
            DocumentContent doc = new DocumentContent()
            {
                Title = string.Format("Layout_{0}", DateTime.Now.ToString()),
                Content = new TextBox()
                {
                    AcceptsReturn = true,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Text = DockManager.LayoutToString()
                }
            };

            doc.Show(DockManager);
            doc.Activate();
        }

        private void ImportLayoutFromDocument(object sender, RoutedEventArgs e)
        {
            if (DockManager.ActiveDocument != null &&
                DockManager.ActiveDocument.Content is TextBox)
            {
                string xml_layout = (DockManager.ActiveDocument.Content as TextBox).Text;
                DockManager.LayoutFromString(xml_layout);
            }
        }

        #region StateChangeLog

        /// <summary>
        /// StateChangeLog Dependency Property
        /// </summary>
        public static readonly DependencyProperty StateChangeLogProperty =
            DependencyProperty.Register("StateChangeLog", typeof(string), typeof(MainWindow),
                new FrameworkPropertyMetadata((string)null));

        /// <summary>
        /// Gets or sets the StateChangeLog property.  This dependency property 
        /// indicates the changelog of dockable contents state.
        /// </summary>
        public string StateChangeLog
        {
            get { return (string)GetValue(StateChangeLogProperty); }
            set { SetValue(StateChangeLogProperty, value); }
        }

        void Log(string message)
        {
            StateChangeLog += string.Format("{0} - {1}{2}", DateTime.Now, message, Environment.NewLine);
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
