using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.WebServer.Files;
using System.IO;
using System.Net;
using System.Text;
using Griffin.Net.Protocols.Http;
using System.Globalization;
using Griffin.WebServer.Modules;
using Griffin.WebServer;

namespace MediaLoaderWPF1.httpServer {
    class MainModule : IWorkerModule {

        private readonly IFileService _fileService;

        public MainModule(IFileService fileService) {
            if (fileService == null) throw new ArgumentNullException("fileService");
            _fileService = fileService;
            Console.WriteLine(@"started main module ");
        }

        [Obsolete("Use 'AllowFileListing")]
        public bool ListFiles {
            get { return AllowFileListing; }
            set { AllowFileListing = value; }
        }


        public bool AllowFileListing { get; set; }

        #region IWorkerModule Members


        public void BeginRequest(IHttpContext context) {
        }

        public void EndRequest(IHttpContext context) {
        }

        public void HandleRequestAsync(IHttpContext context, Action<IAsyncModuleResult> callback) {
            // just invoke the callback synchronously.
            var url = context.Request.Uri.AbsolutePath;
            //Console.WriteLine("top level handeling async, " + url);
            callback(new AsyncModuleResult(context, HandleRequest(context)));
        }

       public ModuleResult HandleRequest(IHttpContext context) {
            var url = context.Request.Uri.AbsolutePath;
            Console.WriteLine(@"top level handeling, " + url);

            // only handle GET and HEAD
            if (!context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase)
                && !context.Request.HttpMethod.Equals("HEAD", StringComparison.OrdinalIgnoreCase))
                return ModuleResult.Continue;

            // serve a directory
            if (AllowFileListing) {
                if (TryGenerateDirectoryPage(context))
                    return ModuleResult.Stop;
            }

            var header = context.Request.Headers["If-Modified-Since"];
            var time = header != null
                           ? DateTime.ParseExact(header, "R", CultureInfo.InvariantCulture)
                           : DateTime.MinValue;


            var fileContext = new FileContext(context.Request, time);
            _fileService.GetFile(fileContext);
            if (!fileContext.IsFound)
                return ModuleResult.Continue;

            if (!fileContext.IsModified) {
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                context.Response.ReasonPhrase = "Was last modified " + fileContext.LastModifiedAtUtc.ToString("R");
                return ModuleResult.Stop;
            }

            if (fileContext.IsGzipSubstitute) {
                context.Response.AddHeader("Content-Encoding", "gzip");
            }

            var mimeType = MimeTypeProvider.Instance.Get(fileContext.Filename);
            if (mimeType == null) {
                context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                context.Response.ReasonPhrase = string.Format("File type '{0}' is not supported.",
                                                                   Path.GetExtension(fileContext.Filename));
                return ModuleResult.Stop;
            }

            context.Response.AddHeader("Last-Modified", fileContext.LastModifiedAtUtc.ToString("R"));
            context.Response.AddHeader("Accept-Ranges", "bytes");
            context.Response.AddHeader("Content-Disposition", "inline;filename=\"" + Path.GetFileName(fileContext.Filename) + "\"");
            context.Response.ContentType = mimeType;
            context.Response.ContentLength = (int)fileContext.FileStream.Length;

            // ranged/partial transfers
            var rangeStr = context.Request.Headers["Range"];
            if (!string.IsNullOrEmpty(rangeStr)) {
                var ranges = new RangeCollection();
                Console.WriteLine(@"getting range for: "+rangeStr);
                if (rangeStr.Equals("bytes=0-"))
                {
                    ranges.Parse("bytes=1-", (int)fileContext.FileStream.Length);
                    //                    rangeStr = "bytes=" + ((int) fileContext.FileStream.Length/358) + "-";
                }
                else
                {
                    ranges.Parse(rangeStr, (int) fileContext.FileStream.Length);
                }
                context.Response.AddHeader("Content-Range", ranges.ToHtmlHeaderValue((int)fileContext.FileStream.Length));
                context.Response.Body = new ByteRangeStream(ranges, fileContext.FileStream);
                context.Response.ContentLength = ranges.TotalLength;
                context.Response.StatusCode = 206;
            } else
                context.Response.Body = fileContext.FileStream;

            // do not include a body when the client only want's to get content information.
            if (context.Request.HttpMethod.Equals("HEAD", StringComparison.OrdinalIgnoreCase) && context.Response.Body != null) {
                context.Response.Body.Dispose();
                context.Response.Body = null;
            }

            return ModuleResult.Stop;
        }

        private bool TryGenerateDirectoryPage(IHttpContext context) {
            if (!_fileService.IsDirectory(context.Request.Uri))
                return false;

            int pos = ListFilesTemplate.IndexOf("{{Files}}", StringComparison.Ordinal);
            if (pos == -1)
                throw new InvalidOperationException("Failed to find '{{Files}}' in the ListFilesTemplate.");
            var newLine = ListFilesTemplate.LastIndexOf("\r\n", pos, StringComparison.Ordinal);
            var spaces = "".PadLeft(pos - newLine - 2); //exclude crlf;

            var sb = new StringBuilder();
            sb.Append(ListFilesTemplate.Substring(0, pos));
            foreach (var file in _fileService.GetFiles(context.Request.Uri)) {
                var fileUri = context.Request.Uri.AbsolutePath + file.Name;
                if (!fileUri.EndsWith("/"))
                    fileUri += "/";
                fileUri += file.Name;

                sb.AppendFormat(@"{4}<tr><td><a href=""{0}"">{1}</a></td><td>{2}</td><td style=""text-align: right"">{3}</td></tr>", fileUri, file.Name,
                                file.LastModifiedAtUtc, file.Size, spaces);
                sb.AppendLine();
            }

            sb.Append(ListFilesTemplate.Substring(pos + 9));


            context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
            return true;
        }

       public static string ListFilesTemplate = @"<html>
    <head>
        <title>Listing files</title>
    <head>
    <body>
        <table>
            <thead>
                <tr>
                    <th>Filename</th>
                    <th>Modified at</th>
                    <th>File size</th>
                </tr>
            <thead>
            <tbody>
                {{Files}}
            </tbody>
        </table>
    </body>
</html>";

        #endregion
    }


}
