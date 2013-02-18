using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Diagnostics;
using System.Linq;

using AvalonDock;
using System.Collections.Generic;

namespace AvalonDockTest
{
    /// <summary>
    /// Interaction logic for Demo.xaml
    /// </summary>
    public partial class Demo : Window
    {
        WindowsFormsHost _PropGridHost = new WindowsFormsHost();

        static Demo()
        {
            //CommandManager.RegisterClassCommandBinding(typeof(Demo), new CommandBinding(ApplicationCommands.Close, new ExecutedRoutedEventHandler(ExecuteCommand), new CanExecuteRoutedEventHandler(OnCanExecuteCommand)));
        }

        public Demo()
        {
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, new ExecutedRoutedEventHandler(ExecuteCommand), new CanExecuteRoutedEventHandler(OnCanExecuteCommand)));

            InitializeComponent();

            //_dockingManager.ApplyTemplate();

            //string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            //     + @"\AvalonDockTest.Layout.xml";
            //if (!File.Exists(path))
            //    return;

            //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            ////_dockingManager.RestoreLayout(fs);

            //fs.Close();

            DependencyPropertyDescriptor prop =
                DependencyPropertyDescriptor.FromProperty(DockableContent.StatePropertyKey.DependencyProperty, typeof(DockableContent));
            prop.AddValueChanged(_propertiesWindow, this.OnLogEventStateChanged);
            prop.AddValueChanged(_explorerWindow, this.OnLogEventStateChanged);
            prop.AddValueChanged(_eventsLogWindow, this.OnLogEventStateChanged);

            _dockingManager.PropertyChanged += new PropertyChangedEventHandler(_dockingManager_PropertyChanged);

            System.Windows.Forms.PropertyGrid _PropGrid = new System.Windows.Forms.PropertyGrid();
            _PropGrid.SelectedObject = _dockingManager;

            _PropGrid.Name = "_PropertyGrid";

            _PropGridHost.Child = _PropGrid;

            this._objectExplorerHost.Content = _PropGridHost;
            //_PropGridHost.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(_PropGridHost_GotKeyboardFocus);
            ////((IKeyboardInputSink)_PropGridHost)

            //DockableContent cnt = new DockableContent();
            //cnt.Title = "Test";
            //cnt.Name = "test";
            //WindowsFormsHost tempHost = new WindowsFormsHost();
            //tempHost.Child = new System.Windows.Forms.TextBox();
            //cnt.Content = tempHost;
            //_dockingManager.Show(cnt, DockableContentState.Docked, AnchorStyle.Left);

            //CommandManager.AddPreviewCanExecuteHandler(_dockingManager, new CanExecuteRoutedEventHandler(OnCanExecuteCommand));
            CommandManager.AddPreviewExecutedHandler(_dockingManager, new ExecutedRoutedEventHandler(ExecuteCommand));
           
        }

        void ExecuteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Close &&
                e.Source is DocumentPane)
            {
                //e.Handled = true;
            }
        }

        void OnCanExecuteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Close)
            {
                if (sender == _dockingManager )
                { 
                
                }
            }
        }

        void _PropGridHost_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            
        }


        private void NewDocuments_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;

            IEnumerable<DemoDocument> docs = _dockingManager.Documents.Cast<DemoDocument>().Where<DemoDocument>(d => d.IsChanged);
            

            int baseCount = _dockingManager.Documents.Length;
            while (i <= 4)
            {
                //DocumentContent doc = new DocumentContent();
                //doc.Title = "Document " + (i + baseCount);
                //doc.InfoTip = "Info tipo for " + doc.Title;
                //doc.ContentTypeDescription = "Sample document";
                //doc.Content = new DemoDocument();
                DemoDocument doc = new DemoDocument();
                doc.Title = "Document " + (i + baseCount);
                doc.InfoTip = "Info tipo for " + doc.Title;
                doc.ContentTypeDescription = "Sample document";
                doc.Closing += new EventHandler<CancelEventArgs>(doc_Closing);
                doc.Closed += new EventHandler(doc_Closed);
                //_documentsHost.Items.Add(doc);
                _dockingManager.MainDocumentPane.Items.Add(doc);

                i++;
            }
        }

        void doc_Closed(object sender, EventArgs e)
        {
            Debug.WriteLine(((DocumentContent)sender).Title + " closed");
        }

        void doc_Closing(object sender, CancelEventArgs e)
        {
            Debug.WriteLine(((DocumentContent)sender).Title + " closing");
        }


        private void ShowProperties_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            if (link.Name == "ShowProperties_AutoHide")
                ShowWindow(_propertiesWindow, DockableContentState.AutoHide);
            else if (link.Name == "ShowProperties_FloatingWindow")
                ShowWindow(_propertiesWindow, DockableContentState.FloatingWindow);
            else
                ShowWindow(_propertiesWindow, DockableContentState.Docked);
        }

        private void ShowExplorer_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            if (link.Name == "ShowExplorer_AutoHide")
                ShowWindow(_explorerWindow, DockableContentState.AutoHide);
            else if (link.Name == "ShowExplorer_FloatingWindow")
                ShowWindow(_explorerWindow, DockableContentState.FloatingWindow);
            else
                ShowWindow(_explorerWindow, DockableContentState.Docked);
        }

        private void ShowEventsLog_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            if (link.Name == "ShowEventsLog_AutoHide")
                ShowWindow(_eventsLogWindow, DockableContentState.AutoHide);
            else if (link.Name == "ShowEventsLog_FloatingWindow")
                ShowWindow(_eventsLogWindow, DockableContentState.FloatingWindow);
            else
                _dockingManager.Show(_eventsLogWindow, DockableContentState.Docked, AnchorStyle.Right);
        }

        private void ShowProperty_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            if (link.Name == "ShowProperty_AutoHide")
                ShowWindow(_objectExplorerHost, DockableContentState.AutoHide);
            else if (link.Name == "ShowProperty_FloatingWindow")
                ShowWindow(_objectExplorerHost, DockableContentState.FloatingWindow);
            else
                _dockingManager.Show(_objectExplorerHost, DockableContentState.Docked, AnchorStyle.Right);
        }

        void ShowWindow(DockableContent contentToShow, DockableContentState desideredState)
        {
            if (desideredState == DockableContentState.AutoHide ||
                desideredState == DockableContentState.FloatingWindow)
            {
                _dockingManager.Show(contentToShow, desideredState);
            }
            else
                _dockingManager.Show(contentToShow);
        }


        void OnLogEventStateChanged(object sender, EventArgs e)
        {
            DockableContent content = sender as DockableContent;

            if (content.ContainerPane is DockablePane)
            {
                _txtLog.AppendText(
                    string.Format("[{0}] '{1}' changed state to '{2}' (Anchor={3})", DateTime.Now.ToLongTimeString(), content.Title, content.State, ((DockablePane)content.ContainerPane).Anchor));
            }
            else
            {
                _txtLog.AppendText(
                    string.Format("[{0}] '{1}' changed state to '{2}'", DateTime.Now.ToLongTimeString(), content.Title, content.State));
            }
            _txtLog.AppendText(Environment.NewLine);
            _txtLog.ScrollToEnd();
        }

        void _dockingManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveContent" && _dockingManager.ActiveContent != null)
            {
                if (_dockingManager.ActiveContent != null)
                {
                    DockablePane containerPane = _dockingManager.ActiveContent.ContainerPane as DockablePane;
                    _txtLog.AppendText(
                        string.Format("[{0}] '{1}' is the active content (Anchor = {2})", DateTime.Now.ToLongTimeString(), _dockingManager.ActiveContent.Title, containerPane != null ? containerPane.Anchor.ToString() : "Document!"));
                    _txtLog.AppendText(Environment.NewLine);
                    _txtLog.ScrollToEnd();
                }
            }
            else if (e.PropertyName == "ActiveDocument" && _dockingManager.ActiveDocument != null)
            {
                if (_dockingManager.ActiveDocument != null)
                {
                    _txtLog.AppendText(
                        string.Format("[{0}] '{1}' is the active document", DateTime.Now.ToLongTimeString(), _dockingManager.ActiveDocument.Title));

                    _txtLog.AppendText(Environment.NewLine);
                    _txtLog.ScrollToEnd();
                }
            }

            Debug.WriteLine(string.Format ("ActiveContent = {0} ActiveDocument = {1}"
                , _dockingManager.ActiveContent == null ? "null" : _dockingManager.ActiveContent.Title
                , _dockingManager.ActiveDocument == null ? "null" : _dockingManager.ActiveDocument.Title));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_dockingManager != null)
                _dockingManager.PropertyChanged -= new PropertyChangedEventHandler(_dockingManager_PropertyChanged);
            
            DependencyPropertyDescriptor prop =
                DependencyPropertyDescriptor.FromProperty(DockableContent.StatePropertyKey.DependencyProperty, typeof(DockableContent));
            prop.RemoveValueChanged(_propertiesWindow, this.OnLogEventStateChanged);
            prop.RemoveValueChanged(_explorerWindow, this.OnLogEventStateChanged);
            prop.RemoveValueChanged(_eventsLogWindow, this.OnLogEventStateChanged);

            string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                + @"\AvalonDockTest.Layout.xml";

            _dockingManager.SaveLayout(path);
        }

        private void SaveLayout_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                + @"\AvalonDockTest.Layout.xml";

            _dockingManager.SaveLayout(path);
        }

        private void RestoreLayout_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                + @"\AvalonDockTest.Layout.xml";
            if (!File.Exists(path))
                return;

            _dockingManager.DeserializationCallback = (s, e_args) =>
                {
                    if (e_args.Name == "_contentDummy")
                    {
                        e_args.Content = new DockableContent();
                        e_args.Content.Title = "Dummy Content";
                        e_args.Content.Content = new TextBlock() { Text = "Content Loaded On Demand!"};
                    }
                };

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            _dockingManager.RestoreLayout(fs);

            fs.Close();
        }

        private void ShowDockingManager_Checked(object sender, RoutedEventArgs e)
        {
            //test the load/unload event putting docking manager out of the logical/visual tree
            TestContainer.Content = ShowDockingManager.IsChecked ? null : _dockingManager;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled=true;
            e.CanExecute = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
 

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button Clicked");
            _dockingManager.Show(docHome);
        }


        private void ChangeColor_Clicked(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;

            switch (item.Tag.ToString())
            {
                case "red":
                    ColorFactory.ChangeColors(Colors.Red);
                    break;
                case "green":
                    ColorFactory.ChangeColors(Colors.DarkGreen);
                    break;
                case "blue":
                    ColorFactory.ChangeColors(Color.FromRgb(93, 136, 230));
                    break;
                case "gray":
                    ColorFactory.ChangeColors(Colors.Black);
                    break;
                case "orange":
                    ColorFactory.ChangeColors(Colors.DarkOrange);
                    break;
                case "lime":
                    ColorFactory.ChangeColors(Colors.Lime);
                    break;
                case "magenta":
                    ColorFactory.ChangeColors(Colors.Magenta);
                    break;
            }
        }


        private void ResetColors_Clicked(object sender, RoutedEventArgs e)
        {
            ((DocumentContent)_dockingManager.ActiveDocument).Close();

            ColorFactory.ResetColors();
        }
    }
}
