using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UserSelectionLibrary.model;

namespace TrayApp {
    class ShowCommand : ICommand
    {
        private MediaLoaderWPF1.MainWindow main;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
//            try
//            {
                // open WPF1
                Console.WriteLine(@"tray clicked");
                if (main == null)
                {
                    main = new MediaLoaderWPF1.MainWindow();
                    main.Closed += Main_Closed;
                }
                main.Show();
//            } catch (Exception exception) {
//                File.AppendAllText(UserFileSelections.logFile, @" tray app startup error " + exception.StackTrace);
//            }
        }

        private void Main_Closed(object sender, EventArgs e)
        {
            main = null;
        }

        public event EventHandler CanExecuteChanged;
    }
}
