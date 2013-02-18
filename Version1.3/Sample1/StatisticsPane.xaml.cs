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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Sample1
{
    /// <summary>
    /// Interaction logic for StatisticsPane.xaml
    /// </summary>
    public partial class StatisticsPane : DockableContent
    {
        public StatisticsPane()
        {
            InitializeComponent();

            Stats = new ObservableCollection<StatsItem>();
            this.DataContext = this;
        }

        public ObservableCollection<StatsItem> Stats { get; private set; }

        protected override void OnManagerChanged(DockingManager oldValue, DockingManager newValue)
        {
            if (oldValue != null)
            {
                oldValue.ActiveDocumentChanged -= new EventHandler(ActiveDocumentChanged);
            }

            if (newValue != null)
            {
                newValue.ActiveDocumentChanged += new EventHandler(ActiveDocumentChanged);
            }

            base.OnManagerChanged(oldValue, newValue);
        }

        Document activeDocument = null;

        void ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (activeDocument != null)
                activeDocument.TextContentChanged -= new EventHandler(UpdateStats);

            activeDocument = Manager != null ? Manager.ActiveDocument as Document : null;

            if (activeDocument != null)
            {
                UpdateStats(this, EventArgs.Empty);
                activeDocument.TextContentChanged += new EventHandler(UpdateStats);
            }
        }

        void UpdateStats(object sender, EventArgs e)
        {
            var statItem = Stats.FirstOrDefault(si => si.Name == activeDocument.Title);

            if (statItem == null)
            { 
                statItem = new StatsItem();
                Stats.Add(statItem);
            }

            statItem.Name = activeDocument.Title;
            statItem.LinesCount = Regex.Matches(activeDocument.TextContent, @"\n", RegexOptions.Multiline).Count;
            statItem.WordsCount = Regex.Matches(activeDocument.TextContent, @"[\S]+").Count;

        }

    }
}
