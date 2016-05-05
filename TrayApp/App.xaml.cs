using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using UserSelectionLibrary.model;

namespace TrayApp {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private TaskbarIcon tb;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            try
            {
                tb = (TaskbarIcon) FindResource("MyNotifyIcon");
                if (tb != null)
                {
                    ShowCommand command = new ShowCommand();
                    tb.DoubleClickCommand = command;
                    tb.LeftClickCommand = command;
                }

                AddToStartupRegistry();
            }
            catch (Exception exception)
            {
//                File.AppendAllText(UserFileSelections.logFile, @" tray app startup error "+exception.StackTrace);
//                Console.Write(exception.StackTrace);
            }

        }

        private void AddToStartupRegistry()
        {
            try
            {
//                RegistryKey rk = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                RegistryKey rk = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                rk.SetValue("ht-tray-app", System.Reflection.Assembly.GetExecutingAssembly().Location);
//                File.AppendAllText(UserFileSelections.logFile, @" added to registery - ");
            }
            catch (Exception e)
            {
//                File.AppendAllText(UserFileSelections.logFile, @" add to registery error: "+e.Message);
//                Console.WriteLine(e.Message);
            }

        }
    }
}
