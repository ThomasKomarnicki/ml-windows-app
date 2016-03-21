using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLoaderWPF1 {
    public class FileSelection {
        private string _directoryPath;

        public string directoryPath {
            get { return _directoryPath; }
            set { _directoryPath = value; }
        }

        private bool _includeSubDirs;

        public bool includeSubDirs {
            get { return _includeSubDirs; }
            set { _includeSubDirs = value; }
        }

        public FileSelection(string directoryPath, bool includeSubDirs) {
            _directoryPath = directoryPath;
            _includeSubDirs = includeSubDirs;
        }
    }
}
