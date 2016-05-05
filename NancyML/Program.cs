using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using log4net;
using Nancy.Hosting.Self;
using UserSelectionLibrary.model;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Nancy;
using TrayApp;

namespace NancyML {
    class Program {


        static void Main(string[] args) {
            //            TrayApp.App app = new TrayApp.App();
            //            app.InitializeComponent();
            //            app.Run();

//            var filename = "TrayApp.exe";
//            //            File.WriteAllText("C:\\PCSyncLog.txt", "installing executable at " + filename);
//
//            ProcessStartInfo install = new ProcessStartInfo();
//            install.Arguments = "install";
//
//            install.FileName = filename;
//            install.WindowStyle = ProcessWindowStyle.Hidden;
//            int exitCode;
//
//            using (Process proc = Process.Start(install)) {
////                proc.WaitForExit();
////
////                exitCode = proc.ExitCode;
//            }


//            NancyServer server = new NancyServer();




            Host host = HostFactory.New(x =>
            {

                x.Service<NancySelfHost>(s =>
                {
                    s.ConstructUsing(name => new NancySelfHost());
                    
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                    s.WithNancyEndpoint(x, c =>
                    {
                        
                        c.AddHost(port: 8988);
//                        c.CreateUrlReservationsOnInstall();
//                        c.OpenFirewallPortsOnInstall("PC-Sync");
                    });
                });

//                x.UseLog4Net();

                
                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.SetDescription("Home Theater Service");
                x.SetDisplayName("Home Theater Service");
                x.SetServiceName("HomeTheaterService");
            });
            host.Run();

        }
    }


    public class NancySelfHost {
//        private NancyHost _nancyHost;

        public void Start() {
//            File.AppendAllText(UserFileSelections.logFile, @"service started - ");

        }

        public void Stop() {
            //            _nancyHost.Stop();
            //            Console.WriteLine("Stopped. Good bye!");
//            File.AppendAllText(UserFileSelections.logFile, @"service stopped - ");
        }
    }

    // create new instance of this to start server
    public class NancyServer
    {
        /*public NancyServer()
        {
            //var uri =new Uri("http://localhost:8988");

            HostFactory.Run(x =>                                    //1
            {
                x.Service<NancySelfHost>(s =>                       //2
                {
                    s.ConstructUsing(name => new NancySelfHost());  //3
                    s.WhenStarted(tc => tc.Start());                //4
                    s.WhenStopped(tc => tc.Stop());                 //5
                });

                x.RunAsNetworkService();              
                x.SetDescription("Sample Topshelf Host");           //7
                x.SetDisplayName("Stuff");                          //8
                x.SetServiceName("stuff");                          //9

            });
        }*/

        public NancyServer()
        {
            var uri = new Uri("http://localhost:8988");

            using (var host = new NancyHost(uri)) {
                host.Start();

                Console.ReadLine();
            }
        }
    }

}
