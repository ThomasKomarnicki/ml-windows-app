using System;
using System.IO;
using log4net;
using Nancy.Hosting.Self;
using NancyML.model;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Nancy;

namespace NancyML {
    class Program {

        static void Main(string[] args) {
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
                x.SetDescription("PC Sync Service");
                x.SetDisplayName("PC Sync Service");
                x.SetServiceName("PCSyncService");
            });
            host.Run();

        }
    }


    public class NancySelfHost {
//        private NancyHost _nancyHost;

        public void Start() {
            File.WriteAllText(UserFileSelections.logFile, "service started");
                
//            var config = new HostConfiguration();
//            config.RewriteLocalhost = false;
//            _nancyHost = new NancyHost(config,new Uri("http://localhost:8988"));
//            _nancyHost.Start();
//
//            LogManager.GetLogger("dog").Debug("debug test");
        }

        public void Stop() {
            //            _nancyHost.Stop();
            //            Console.WriteLine("Stopped. Good bye!");
            File.WriteAllText(UserFileSelections.logFile, "service stopped");
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

//                Console.ReadLine();
            }
        }
    }

}
