using System;
using System.IO;
using System.Text.RegularExpressions;
using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;
using NancyML.model;
using NancyML.video;

namespace NancyML.Modules {
    public class IndexModule : NancyModule
    {

        private UserFileSelections _userFileSelections;

        public IndexModule()
        {
            if (_userFileSelections == null)
            {
                _userFileSelections = new UserFileSelections();
                _userFileSelections.LoadFromFile();
                _userFileSelections.ReloadResourcesInFileSelections();
            }

            Get["/media/{path*}"] = parameters => {
                string url = this.Context.Request.Url.Path;
                Console.WriteLine(@"gettin file");
                //                var response = new GenericFileResponse("C:\\Users\\tdk10\\Downloads\\dogs.mp4", Context);
                //                return response;
                //                return Response.AsFile("C:\\Users\\tdk10\\Downloads\\dogs.mp4");

                string realPath = _userFileSelections.GetRealPathOfFile("/"+parameters.path);

                try
                {
                    var file = new FileStream(realPath, FileMode.Open);
                    var fileName = Path.GetFileName(realPath);

                    var response = new StreamResponse(() => file,
                        MimeTypes.GetMimeType(fileName));
                    return response.AsAttachment(fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;
            };

            Get["/thumbnail/{fileName*}"] = parameters =>
            {
                string filename = parameters.fileName;
                string realPath = Path.Combine(VideoThumbnailConverter.GetThumbnailDirectory(), filename);

                try
                {
                    var file = new FileStream(realPath, FileMode.Open);
                    var fileName = Path.GetFileName(realPath);

                    var response = new StreamResponse(() => file,
                        MimeTypes.GetMimeType(fileName));
                    //return response.AsAttachment(fileName);
                    return response;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;
            };

            Get["/ping"] = parameters => Response.AsText("{\"status\":200, \"message\":\"OK\"}", "application/json");

            Get["/data"] = _ =>
            {
                _userFileSelections.LoadFromFile();
                _userFileSelections.ReloadResourcesInFileSelections();
//                File.WriteAllText(UserFileSelections.logFile, "getting data from: "+ UserFileSelections.appDataDir);
                
                Console.WriteLine(@"got data");
                var data = JsonConvert.SerializeObject(new SelectionsWrapper(_userFileSelections.fileSelections));
                data = StripJsonOfFileData(data);
                return Response.AsText(data, "application/json");
            };
        }

        private string StripJsonOfFileData(string jsonData)
        {
            jsonData = Regex.Replace(jsonData, "\"thumbnailLocation(.*?)jpg\",?", "");
//            return jsonData;
            return Regex.Replace(jsonData, "\"directoryPath(.*?)\",", "");
        }


    }
}