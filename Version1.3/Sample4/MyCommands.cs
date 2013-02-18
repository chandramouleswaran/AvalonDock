using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Sample4
{
    public static class MyCommands
    {
        #region TestCommand

        /// <summary>
        /// The TestCommand command .
        /// </summary>
        public static RoutedUICommand TestCommand
            = new RoutedUICommand("Test Command", "TestCommand", typeof(MyCommands));

        #endregion


    }
}
