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
using AvalonDock;
using System.IO;
using System.Collections.ObjectModel;

namespace AvalonDockTest
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public Window4()
        {
            MyDocs = new ObservableCollection<DocumentContent>();
            this.DataContext = this;

            InitializeComponent();



        }

        public ObservableCollection<DocumentContent> MyDocs { get; set; }

        private void dockingManager_Loaded(object sender, RoutedEventArgs e)
        {
            //string xmlLayout =
            //    "<DockingManager>" +
            //      "<ResizingPanel Orientation=\"Horizontal\">" +
            //      "  <DockablePane ResizeWidth=\"0.2125\" Anchor=\"Left\">" +
            //      "    <DockableContent Name=\"MyUserControl1\" AutoHide=\"false\" />" +
            //      "  </DockablePane>" +
            //      "  <DockablePane Anchor=\"Left\">" +
            //      "    <DockableContent Name=\"MyUserControl2\" AutoHide=\"false\" />" +
            //      "  </DockablePane>" +
            //      "</ResizingPanel>" +
            //      "<Hidden />" +
            //      "<Windows />" +
            //    "</DockingManager>";

            //StringReader sr = new StringReader(xmlLayout);
            //dockingManager.RestoreLayout(sr);


        }

        void doc_Closed(object sender, EventArgs e)
        {
            
        }

        void doc_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }


        private void dockingManager_RequestDocumentClose(object sender, RequestDocumentCloseEventArgs e)
        {
            MyDocs.Remove(e.DocumentToClose);
        }

        private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            MyDocs.Clear();
        }

        private void btnAddDocuments_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                DemoDocument doc = new DemoDocument();
                doc.Title = "Document " + (i);
                doc.InfoTip = "Info tipo for " + doc.Title;
                doc.ContentTypeDescription = "Sample document";
                doc.Closing += new EventHandler<System.ComponentModel.CancelEventArgs>(doc_Closing);
                doc.Closed += new EventHandler(doc_Closed);
                MyDocs.Add(doc);
            }
        }

        private void btnRemoveCurrent_Click(object sender, RoutedEventArgs e)
        {
            MyDocs.Remove(dockingManager.ActiveDocument as DemoDocument);
        }

    }
}
