using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AvalonDockTest
{
    public class EmployeeInfo
    {
        string name;

        public string FirstName
        {
            get { return name; }
            set { name = value; }
        }
        string lastname;

        public string LastName
        {
            get { return lastname; }
            set { lastname = value; }
        }

        int no;
        public int EmployeeNumber
        {
            get { return no; }
            set { no = value; }
        }

        public EmployeeInfo(int n, string fn, string ln)
        {
            EmployeeNumber = n;
            FirstName = fn;
            LastName = ln;
        }
    }

    public class EmployeeInfoCollection : ObservableCollection<EmployeeInfo>
    {
        public EmployeeInfoCollection()
        {
            Add(new EmployeeInfo(1, "Name1", "LastName1"));
            Add(new EmployeeInfo(2, "Name2", "LastName2"));
            Add(new EmployeeInfo(3, "Name3", "LastName3"));
            Add(new EmployeeInfo(4, "Name4", "LastName4"));
        }
    }
}
