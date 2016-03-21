using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLoaderWPF1.model {

    class SelectionsWrapper {
        public List<FileSelection> selections;

        public SelectionsWrapper(List<FileSelection> selections) {
            this.selections = selections;
        }
    }
}
