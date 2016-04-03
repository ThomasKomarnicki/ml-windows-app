using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace NancyML.model {
    public class UserFileSelections {

//        private readonly string fileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MediaLoader\\", "user_file_selections.json");
        public static readonly string fileLocation = Path.Combine("C:\\PCSync\\", "user_file_selections.json");
//        private readonly string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MediaLoader\\";
        public static readonly string appDataDir = "C:\\PCSync\\";

        public List<FileSelection> fileSelections;

        public UserFileSelections() {

        }


        public void SaveToFile() {
            if (!File.Exists(fileLocation)) {
                Directory.CreateDirectory(appDataDir);
            }
            // File.WriteAllText(fileLocation, "{\"selections\":"+JsonConvert.SerializeObject(fileSelections)+"}");
            File.WriteAllText(fileLocation,  JsonConvert.SerializeObject(new SelectionsWrapper(fileSelections)));
        }

        public void LoadFromFile() {
            if (File.Exists(fileLocation)) { 
                string fileContents = File.ReadAllText(fileLocation);
                fileSelections = JsonConvert.DeserializeObject<SelectionsWrapper>(fileContents).resourceGroups;

            } else {
                fileSelections = new List<FileSelection>();
            }
        }

        public int Remove(FileSelection fileSelection) {
            for(int i = 0; i < fileSelections.Count; i++) {
                if(Object.ReferenceEquals(fileSelections.ElementAt(i), fileSelection)) {
                    fileSelections.RemoveAt(i);
                    return i;
                }
            }

            return -1;
        }

        public string GetRealPathOfFile(string filePath) {
            foreach(FileSelection fileSelection  in fileSelections) {
                string groupName = fileSelection.groupName;

                if (filePath.Length > groupName.Length)
                {
                    string filePathName = filePath.Substring(1, groupName.Length);
                    if (groupName.Equals(filePathName))
                    {
                        foreach (Resource resource in fileSelection.resourceList)
                        {
                            if (Path.GetFileName(filePath).Equals(resource.name))
                            {
                                string dir =fileSelection.directoryPath.Remove(fileSelection.directoryPath.Length -filePathName.Length);
                                return dir + resource.localLocation;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public void ReloadResourcesInFileSelections() {
            foreach(FileSelection fileSelection in fileSelections) {
                fileSelection.ReloadResourcesInFileSelection();
            }
        }
    }
}
