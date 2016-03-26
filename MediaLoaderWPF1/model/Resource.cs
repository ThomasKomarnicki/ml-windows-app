using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace MediaLoaderWPF1.model {

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
