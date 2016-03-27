using System;
using System.IO;
using NancyML.video;
using Newtonsoft.Json;


namespace NancyML.model {

    public class Resource {

        public string name;

        [JsonIgnore]
        public string localLocation;

        [JsonIgnore] public string FullLocalLocation;

        public string location;

        public string thumbnailLocation;

        public string thumbnailPath
        {
            get { return "thumbnail/" + Path.GetFileName(thumbnailLocation); }
        }

        public Resource()
        {
            
        }
        public Resource(string location) {
            this.localLocation = location;
            this.location = location.Replace("\\", "/");
            this.name = Path.GetFileName(location);
        }

        public Resource(string location, string file) : this(location) {
            FullLocalLocation = file;
            if (VideoThumbnailConverter.ThumbnailExists(FullLocalLocation))
            {
                thumbnailLocation = VideoThumbnailConverter.GetThumbnailPathForFile(FullLocalLocation);
            }
        }
    }
}
