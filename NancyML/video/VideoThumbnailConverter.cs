using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NReco.VideoConverter;

namespace NancyML.video {

    public class VideoThumbnailConverter
    {

        private string _videoPath;

        public VideoThumbnailConverter(string videoFilePath)
        {
            _videoPath = videoFilePath;
            string thumbnailDirectory = GetThumbnailDirectory();
            if (!Directory.Exists(thumbnailDirectory)) {
                Directory.CreateDirectory(thumbnailDirectory);
            }

        }

        public string CreateThumbnail()
        {
            string thumbnailPath = GetThumbnailPathForFile(_videoPath);
            var ffMpeg = new FFMpegConverter();
            ffMpeg.GetVideoThumbnail(_videoPath, File.Create(thumbnailPath), 8);

            return thumbnailPath;
        }

        public static string GetThumbnailPathForFile(string videoFile)
        {
            var hashCode = videoFile.GetHashCode();
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MediaLoader\\.thumbnail\\", "."+hashCode +"_thumbnail.jpg");
        }

        public static bool ThumbnailExists(string videoFile)
        {
            return File.Exists(GetThumbnailPathForFile(videoFile));
        }

        public static string GetThumbnailDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MediaLoader\\.thumbnail\\";
        }

    }
}
