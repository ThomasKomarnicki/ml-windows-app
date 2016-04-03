using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using NancyML;

namespace PCSyncService {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var server = new NancyServer();

            if (!Environment.UserInteractive)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new PCSyncService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                PCSyncService service = new PCSyncService();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
        }
    }
}
