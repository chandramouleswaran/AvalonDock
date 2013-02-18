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

namespace Sample4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MyDocuments = new ObservableCollection<DocumentContent>();
            BuildPeopleTree();

            InitializeComponent();

            DataContext = this;
            
        }

        public ObservableCollection<DocumentContent> MyDocuments { get; private set; }

        private void CreateNewDocument(object sender, RoutedEventArgs e)
        {

            string baseDocTitle = "MyDocument";
            int i = 1;
            string title = baseDocTitle + i.ToString();

            while (dockManager.Documents.Any(d => d.Title == title))
            {
                i++;
                title = baseDocTitle + i.ToString();
            }

            DocumentContent doc = new DocumentContent() { Title = title };
            doc.Show(dockManager);

            //MyDocuments.Add(new DocumentContent() { Title = title });
        }

        private void ClearDocumentsList(object sender, RoutedEventArgs e)
        {
            MyDocuments.Clear();

            foreach (var doc in dockManager.Documents.ToArray())
                doc.Close();
        }


        void BuildPeopleTree()
        {
            People = new ObservableCollection<Person>();
            People.Add(new Person() { Name = "Person1" });
            People.Add(new Person() { Name = "Person2" });
            People.Add(new Person() { Name = "Person3" });
            People[0].Subordinates.Add(new Person() { Name = "Person4" });
            People[0].Subordinates.Add(new Person() { Name = "Person5" });
            People[1].Subordinates.Add(new Person() { Name = "Person6" });
            People[1].Subordinates.Add(new Person() { Name = "Person7" });        
        }


        public ObservableCollection<Person> People
        {
            get { return (ObservableCollection<Person>)GetValue(PeopleProperty); }
            set { SetValue(PeopleProperty, value); }
        }

        public static readonly DependencyProperty PeopleProperty =
            DependencyProperty.Register("People", typeof(ObservableCollection<Person>), typeof(MainWindow), new UIPropertyMetadata(null));

        private void OnCanExecuteTestCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnExecutedTestCommand(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Executed!");
        }

    }
}
