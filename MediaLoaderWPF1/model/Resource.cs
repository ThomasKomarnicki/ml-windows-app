using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MediaLoaderWPF1.model {

    public class Resource {

        public String name;
        public String location;

        public Resource(String location) {
            this.location = location;
            this.name = Path.GetFileName(location);
        }

    }
}
