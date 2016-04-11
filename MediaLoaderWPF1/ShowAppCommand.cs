using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MediaLoaderWPF1 {
    class ShowAppCommand : ICommand {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine(@"system tray clicked");
        }

        public event EventHandler CanExecuteChanged;
    }
}
