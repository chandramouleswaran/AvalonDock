using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;

namespace AvalonDock.WinFormsTestApp
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            
        }


        DockingManager _dockingManager = new DockingManager();
        protected override void OnLoad(EventArgs e)
        {
            XmlLayoutSerializer serializer = new XmlLayoutSerializer(_dockingManager);

            serializer.LayoutSerializationCallback += (s, args) =>
                {
                    switch (args.Model.ContentId)
                    {
                        case "toolWindow1":
                            args.Content = new System.Windows.Controls.TextBlock() { Text = args.Model.ContentId };
                            break;
                        default:
                            args.Content = new System.Windows.Controls.TextBox() { Text = args.Model.ContentId };
                            break;
                    }

                };

            serializer.Deserialize(
                new System.IO.StringReader(
                AvalonDock.WinFormsTestApp.Properties.Settings.Default.DefaultLayout));


            //LayoutDocument doc = new LayoutDocument() { Title = "test" };
            //dockingManager.Layout.Descendents().OfType<LayoutDocumentPane>().First().Children.Add(doc);

            dockingManagerHost.Child = _dockingManager;
            
            base.OnLoad(e);
        }


        private void menuItemAero_Click(object sender, EventArgs e)
        {
            _dockingManager.Theme = new AvalonDock.Themes.AeroTheme();
        }

        private void menuItemVS2010_Click(object sender, EventArgs e)
        {
            _dockingManager.Theme = new AvalonDock.Themes.VS2010Theme();
        }

        private void menuItemDefault_Click(object sender, EventArgs e)
        {
            _dockingManager.Theme = null;
        }

    }
}
