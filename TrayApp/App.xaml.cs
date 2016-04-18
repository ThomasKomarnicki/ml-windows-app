using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;

namespace TrayApp {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private TaskbarIcon tb;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            tb = (TaskbarIcon)FindResource("MyNotifyIcon");
            if (tb != null)
            {
                ShowCommand command = new ShowCommand();
                tb.DoubleClickCommand = command;
                tb.LeftClickCommand = command;
            }

            AddToStartupRegistry();

        }

        private void AddToStartupRegistry()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            rk.SetValue("PC-Sync", System.Reflection.Assembly.GetExecutingAssembly().Location);

        }
    }
}
