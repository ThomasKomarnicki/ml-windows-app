using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLoaderWPF1.model {

    class SelectionsWrapper {
        public List<FileSelection> resourceGroups;

        public SelectionsWrapper(List<FileSelection> selections) {
            this.resourceGroups = selections;
        }
    }
}
