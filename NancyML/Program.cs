using System;
using log4net;
using Nancy.Hosting.Self;
using Topshelf;
using Topshelf.Nancy;

namespace NancyML {
    class Program {

        static void Main(string[] args) {
            //            NancyServer server = new NancyServer();

            HostFactory.Run(x =>
            {
                x.Service<NancySelfHost>(s =>
                {
                    s.ConstructUsing(name => new NancySelfHost());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
//                    s.WithNancyEndpoint(x, c =>
//                    {
//                        c.AddHost(port: 8988);
//                        c.CreateUrlReservationsOnInstall();
//                        c.OpenFirewallPortsOnInstall("PC Sync");
//                    });
                });

                x.UseLog4Net();

                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.SetDescription("PC Sync Self Host");
                x.SetDisplayName("PC Sync Self");
                x.SetServiceName("PCSyncServer");
            });
            
        }
    }


    public class NancySelfHost {
        private NancyHost _nancyHost;

        public void Start() {
            _nancyHost = new NancyHost(new Uri("http://localhost:8988"));
            _nancyHost.Start();

            LogManager.GetLogger("dog").Debug("debug test");
        }

        public void Stop() {
            _nancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
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

    /*public class ServerWorker
    {
        public void DoWork() {
            var uri = new Uri("http://localhost:8988");
            using (var host = new NancyHost(uri)) {
                host.Start();

                Console.WriteLine("Your application is running on " + uri);
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }
            while (!_shouldStop) {
                
            }

        }

        public void RequestStop() {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data 
        // member will be accessed by multiple threads. 
        private volatile bool _shouldStop;
    }*/
}
