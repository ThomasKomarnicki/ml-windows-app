using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLoaderWPF1.model;
using Griffin.WebServer.Files;
using System.Net;
using System.Net.Sockets;
using Griffin.Logging;
using Griffin.Logging.Loggers;
using Griffin.WebServer;
using Griffin.WebServer.Modules;

namespace MediaLoaderWPF1.httpServer {
    class GriffinHttpServer {

        public GriffinHttpServer(UserFileSelections userFileSelections) {
            var fileService = new MediaFileService(userFileSelections);

            var moduleManager = new ModuleManager();

            var dataModule = new DataModule(userFileSelections);
            moduleManager.Add(dataModule);

            // Create the file module and allow files to be listed.
            var module = new MainModule(fileService) { AllowFileListing = true };


            moduleManager.Add(module);

            // And start the server.
            var server = new HttpServer(moduleManager);
            server.Start(IPAddress.Any, 8988);
        }
    }
}
