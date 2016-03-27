using System;
using System.IO;
using Newtonsoft.Json;


namespace NancyML.model {

    public class Resource {

        public string name;

        [JsonIgnore]
        public string localLocation;

        public string location;

        public Resource(String location) {
            this.localLocation = location;
            this.location = location.Replace("\\", "/");
            this.name = Path.GetFileName(location);
        }

    }
}
