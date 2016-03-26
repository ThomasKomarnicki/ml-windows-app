using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.WebServer.Modules;
using Griffin.WebServer;
using System.IO;
using MediaLoaderWPF1.model;
using Newtonsoft.Json;

namespace MediaLoaderWPF1.httpServer {
    class DataModule : IWorkerModule {

        private UserFileSelections _userFileSelections;

        public DataModule(UserFileSelections userFileSelections) {
            _userFileSelections = userFileSelections;
        }

        public void BeginRequest(IHttpContext context) {
            
        }

        public void EndRequest(IHttpContext context) {
            
        }

        public void HandleRequestAsync(IHttpContext context, Action<IAsyncModuleResult> callback) {
            // Since this module only supports sync
            callback(new AsyncModuleResult(context, HandleRequest(context)));
        }

        public ModuleResult HandleRequest(IHttpContext context) {

            Console.WriteLine("data module handling " + context.Request.Uri.AbsolutePath);
            string url = context.Request.Uri.AbsolutePath;

            if (url.StartsWith("/ping")) { 

                string data = "{\"status\":200, \"message\":\"OK\"}";

                ConfigContextForTextData(context, data);

                /*context.Response.AddHeader("Content-Type", "application/json");
                context.Response.ContentCharset = Encoding.UTF8;
                context.Response.Body = new MemoryStream();

                var writer = new StreamWriter(context.Response.Body, Encoding.UTF8);
                writer.WriteLine(data);

                context.Response.Body.Position = 0;

                writer.Flush();*/
            }else if (url.StartsWith("/data")) {
                _userFileSelections.reloadResourcesInFileSelections();
                string data = JsonConvert.SerializeObject(new SelectionsWrapper(_userFileSelections.fileSelections));
                ConfigContextForTextData(context, data);
            } else {
                return ModuleResult.Continue;
            }

            return ModuleResult.Stop;
        }

        private void ConfigContextForTextData(IHttpContext context, string data) {
            context.Response.AddHeader("Content-Type", "application/json");
            context.Response.ContentCharset = Encoding.UTF8;
            context.Response.Body = new MemoryStream();

            var writer = new StreamWriter(context.Response.Body, Encoding.UTF8);
            writer.WriteLine(data);

            context.Response.Body.Position = 0;

            writer.Flush();
        }
    }
}
