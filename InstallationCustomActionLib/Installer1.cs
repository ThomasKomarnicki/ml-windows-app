using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InstallationCustomActionLib {
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer {
        public Installer1() {
            InitializeComponent();
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver) {
            base.Install(stateSaver);
            // todo install and run service open main window

        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState) {
            base.Commit(savedState);

            string path = this.Context.Parameters["targetdir"];
            var filename = path + "\\NancyML.exe";
//            File.WriteAllText("C:\\PCSyncLog.txt", "installing executable at " + filename);

            ProcessStartInfo install = new ProcessStartInfo();
            install.Arguments = "install";

            install.FileName = filename;
            install.Verb = "runas";
            install.WindowStyle = ProcessWindowStyle.Hidden;
            int exitCode;

            using (Process proc = Process.Start(install)) {
                proc.WaitForExit();

                exitCode = proc.ExitCode;
            }

            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = "start";

            start.FileName = filename;
            install.Verb = "runas";
            start.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process proc = Process.Start(start)) {
                proc.WaitForExit();

                exitCode = proc.ExitCode;
            }

        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState) {
            base.Rollback(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState) {
            base.Uninstall(savedState);

            string path = this.Context.Parameters["targetdir"];
            var filename = path + "\\NancyML.exe";

            ProcessStartInfo uninstall = new ProcessStartInfo();
            uninstall.Arguments = "uninstall";

            uninstall.FileName = filename;
            uninstall.WindowStyle = ProcessWindowStyle.Hidden;
            int exitCode;

            using (Process proc = Process.Start(uninstall)) {
                proc.WaitForExit();

                exitCode = proc.ExitCode;
            }
        }
    }
}
