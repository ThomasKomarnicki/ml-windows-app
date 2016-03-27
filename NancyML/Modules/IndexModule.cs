using System;
using System.IO;
using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;
using NancyML.model;

namespace NancyML.Modules {
    public class IndexModule : NancyModule
    {

        private readonly UserFileSelections _userFileSelections;

        public IndexModule(UserFileSelections userFileSelections)
        {
            _userFileSelections = userFileSelections;

            Get["/user_media/{path*}"] = parameters => {
                string url = this.Context.Request.Url.Path;
                Console.WriteLine(@"gettin file");
                //                var response = new GenericFileResponse("C:\\Users\\tdk10\\Downloads\\dogs.mp4", Context);
                //                return response;
                //                return Response.AsFile("C:\\Users\\tdk10\\Downloads\\dogs.mp4");

                string realPath = _userFileSelections.GetRealPathOfFile(parameters.path);

                var file = new FileStream(realPath, FileMode.Open);
                string fileName = "dogs.mp4";

                var response = new StreamResponse(() => file, MimeTypes.GetMimeType(fileName));
                return response.AsAttachment(fileName);
            };

            Get["/ping"] = parameters => Response.AsText("{\"status\":200, \"message\":\"OK\"}", "application/json");

            Get["/data"] = _ =>
            {
                Console.WriteLine(@"got data");
                var data = JsonConvert.SerializeObject(new SelectionsWrapper(_userFileSelections.fileSelections));
                return Response.AsText(data, "application/json");
            };

        }
    }
}