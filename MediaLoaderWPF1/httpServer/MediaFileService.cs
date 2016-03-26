using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.WebServer.Files;
using System.IO;
using System.Security.Policy;
using MediaLoaderWPF1.model;

namespace MediaLoaderWPF1.httpServer {
    class MediaFileService : IFileService {

        private UserFileSelections _userFileSelections;

        public MediaFileService(UserFileSelections userFileSelections) {
            _userFileSelections = userFileSelections;
        }

        public IEnumerable<string> GetDirectories(Uri uri) {
            var path = GetFullPath(uri);
            if (path == null || !Directory.Exists(path))
                yield break;

            yield return "..";
            foreach (var directory in Directory.GetDirectories(path)) {
                if (directory.StartsWith("."))
                    continue;

                yield return directory.Remove(0, path.Length);
            }
        }

        public bool GetFile(FileContext context) {
            if (!context.Request.Uri.AbsolutePath.StartsWith("/media")) { 
                return false;
            }

            Console.WriteLine("**Griffin** getting file for " + context.Request.Uri.AbsolutePath);
            var fullPath = GetFullPath(context.Request.Uri);
            if (fullPath == null || !File.Exists(fullPath))
                return false;

            var streamPath = fullPath;

            var date = File.GetLastWriteTimeUtc(fullPath);

            // browser ignores second fractions.
            date = date.AddTicks(-(date.Ticks % TimeSpan.TicksPerSecond));

            if (date <= context.BrowserCacheDate) {
                context.SetNotModified(fullPath, date);
                return true;
            }

            var stream = new FileStream(streamPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            context.SetFile(fullPath, stream, date);
            return true;
        }

        private string GetFullPath(Uri uri) {

            if(!uri.AbsolutePath.StartsWith("/media")) {
                return null;
            }
            string filePath = uri.AbsolutePath.Substring(6);

            //Console.WriteLine("Processing media request for " + filePath);

            string fileLocation = _userFileSelections.getRealPathOfFile(filePath);
            return fileLocation.Replace('/', Path.DirectorySeparatorChar);

            /*if (!uri.AbsolutePath.StartsWith(_rootUri))
                return null;

            var relativeUri = Uri.UnescapeDataString(uri.AbsolutePath.Remove(0, _rootUri.Length));
            return Path.Combine(_basePath, relativeUri.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));*/
        }

        public IEnumerable<FileInformation> GetFiles(Uri uri) {
            var path = GetFullPath(uri);
            if (path == null || !Directory.Exists(path))
                yield break;

            foreach (var file in Directory.GetFiles(path, "*.*")) {
                var mimeType = MimeTypeProvider.Instance.Get(Path.GetFileName(file));
                if (mimeType == null)
                    continue;

                var info = new FileInfo(file);
                yield return new FileInformation {
                    LastModifiedAtUtc = info.LastWriteTimeUtc,
                    Name = Path.GetFileName(file),
                    Size = (int)info.Length
                };
            }
        }

        public bool IsDirectory(Uri uri) {
            var path = GetFullPath(uri);
            return path != null && Directory.Exists(path);

        }
    }
}
