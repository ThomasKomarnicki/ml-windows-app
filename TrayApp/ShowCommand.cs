﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
            // open WPF1
            Console.WriteLine(@"tray clicked");
            if (main == null)
            {
                main = new MediaLoaderWPF1.MainWindow();
                main.Closed += Main_Closed;
            }
            main.Show();
        }

        private void Main_Closed(object sender, EventArgs e)
        {
            main = null;
        }

        public event EventHandler CanExecuteChanged;
    }
}
