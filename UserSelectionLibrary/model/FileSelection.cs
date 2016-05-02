using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using UserSelectionLibrary.video;

namespace UserSelectionLibrary.model {
    public class FileSelection {

        [JsonIgnore]
        private List<string> EXTENSIONS = new List<string> { ".mp4", ".avi", ".mpeg", ".mpg", ".webm",".mkv", ".flv" };

        private string extensionsRegex = @".*\.((?i)(mp4|avi|mpg|mpeg|webm|mkv|flv))";


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

        private int _thumbnailsCreated = 0;

        [JsonIgnore]
        public int ThumbnailsCreated
        {
            get { return _thumbnailsCreated; }
            set { _thumbnailsCreated = value; }
        }

        public FileSelection()
        {
            
        }

        public FileSelection(string directoryPath/*, bool includeSubDirs*/) {
            _directoryPath = directoryPath;
            _includeSubDirs = true;

            _groupName = Path.GetFileName(_directoryPath);

            //ReloadResourcesInFileSelection();

        }

        public void ReloadResourcesInFileSelection() {
            _resourceList = new List<Resource>();
//            SearchOption searchOption = SearchOption.AllDirectories;
//            string[] files = Directory.GetFiles(_directoryPath, "*", searchOption);

            ApplyAllFiles(directoryPath, ProcessFile);

//            Console.WriteLine("***loading files from " + _directoryPath + "***");

//            foreach (string file in files) {
//                string extension = Path.GetExtension(file);
//
//                if(Regex.Match(extension, extensionsRegex).Success) { 
////                if (EXTENSIONS.Contains(extension, StringComparer.OrdinalIgnoreCase)) {
//                    // add smart file path to _fileUrls
//                    // replace path with _groupName up to _directoryPath
//                    string fileUrl = file.Replace(_directoryPath, _groupName);
//                    Resource resource = new Resource(fileUrl, file);
//                    _resourceList.Add(resource);
//                    Console.WriteLine("fileUrl = " + resource.localLocation);
//                }
//            }
        }

        private void ProcessFile(string path)
        {
            string extension = Path.GetExtension(path);

            if (Regex.Match(extension, extensionsRegex).Success) {
                //                if (EXTENSIONS.Contains(extension, StringComparer.OrdinalIgnoreCase)) {
                // add smart file path to _fileUrls
                // replace path with _groupName up to _directoryPath
                string fileUrl = path.Replace(_directoryPath, _groupName);
                Resource resource = new Resource(fileUrl, path);
                _resourceList.Add(resource);
                Console.WriteLine("fileUrl = " + resource.localLocation);
            }
        }
        private void ApplyAllFiles(string folder, Action<string> fileAction) {
            foreach (string file in Directory.GetFiles(folder)) {
                fileAction(file);
            }
            foreach (string subDir in Directory.GetDirectories(folder)) {
                try {
                    ApplyAllFiles(subDir, fileAction);
                } catch {
                    // swallow, log, whatever
                }
            }
        }

        public bool HasAllThumbnails()
        {
            if (_includeSubDirs && _thumbnailsCreated == 2)
            {
                return true;
            }
            if (!_includeSubDirs && _thumbnailsCreated == 1)
            {
                return true;
            }
            return false;
        }

        public void CreateThumbnails()
        {
            if (_includeSubDirs && _thumbnailsCreated == 2)
            {
                return;
            }
            else if (!_includeSubDirs && _thumbnailsCreated == 1)
            {
                return;
            } 
            else if (_includeSubDirs && _thumbnailsCreated != 2)
            {
                _thumbnailsCreated = 2;
            } 
            else if (!_includeSubDirs && _thumbnailsCreated == 0)
            {
                _thumbnailsCreated = 1;
            }
            ReloadResourcesInFileSelection();

            foreach (var resource in _resourceList)
            {
                if (!VideoThumbnailConverter.ThumbnailExists(resource.FullLocalLocation))
                {
                    var thumbnailConverter = new VideoThumbnailConverter(resource.FullLocalLocation);
                    resource.thumbnailLocation = thumbnailConverter.CreateThumbnail();
                    Console.WriteLine("created thumbnail at "+resource.thumbnailLocation);
                }

            }

        }
    }
}
