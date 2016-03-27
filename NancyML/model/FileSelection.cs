using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace NancyML.model {
    public class FileSelection {

        [JsonIgnore]
        private List<string> EXTENSIONS = new List<string> { ".mp4", ".avi", ".mpeg", ".mpg", ".webm",".mkv", ".flv" };
        
        private string _directoryPath;

        [JsonIgnore]
        public string directoryPath {
            get { return _directoryPath; }
            set { _directoryPath = value; }
        }

        private bool _includeSubDirs;

        public bool includeSubDirs {
            get { return _includeSubDirs; }
            set { _includeSubDirs = value; }
        }

        private string _groupName;
        
        public string groupName {
            get { return _groupName; }
            set { _groupName = value; }
        }


        /*private List<string> _fileUrls;

        public List<string> fileUrls {
            get { return _fileUrls; }
        }*/

        private List<Resource> _resourceList;

        public List<Resource> resourceList {
            get { return _resourceList; }
        }

        public FileSelection(string directoryPath, bool includeSubDirs) {
            _directoryPath = directoryPath;
            _includeSubDirs = includeSubDirs;

            _groupName = Path.GetFileName(_directoryPath);

            //ReloadResourcesInFileSelection();

        }

        public void ReloadResourcesInFileSelection() {
            _resourceList = new List<Resource>();
            string[] files = Directory.GetFiles(_directoryPath);

            Console.WriteLine("***loading files from " + _directoryPath + "***");

            foreach (string file in files) {
                string extension = Path.GetExtension(file);

                if (EXTENSIONS.Contains(extension, StringComparer.OrdinalIgnoreCase)) {
                    // add smart file path to _fileUrls
                    // replace path with _groupName up to _directoryPath
                    string fileUrl = file.Replace(_directoryPath, _groupName);
                    Resource resource = new Resource(fileUrl);
                    _resourceList.Add(resource);
                    Console.WriteLine("fileUrl = " + resource.localLocation);
                }
            }
        }
    }
}
