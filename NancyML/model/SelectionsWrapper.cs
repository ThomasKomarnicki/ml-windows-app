using System.Collections.Generic;

namespace NancyML.model {

    public class SelectionsWrapper {
        public List<FileSelection> resourceGroups;

        public SelectionsWrapper(List<FileSelection> selections) {
            this.resourceGroups = selections;
        }
    }
}
