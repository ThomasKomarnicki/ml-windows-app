using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace MediaLoaderWPF1.model {
    class UserFileSelections {

        private string fileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MediaLoader\\", "user_file_selections.json");
        private string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MediaLoader\\";

        public List<FileSelection> fileSelections;

        public UserFileSelections() {

        }


        public void saveToFile() {
            if (!File.Exists(fileLocation)) {
                Directory.CreateDirectory(appDataDir);
            }
            // File.WriteAllText(fileLocation, "{\"selections\":"+JsonConvert.SerializeObject(fileSelections)+"}");
            File.WriteAllText(fileLocation,  JsonConvert.SerializeObject(new SelectionsWrapper(fileSelections)));
        }

        public void loadFromFile() {
            if (File.Exists(fileLocation)) { 
                String fileContents = File.ReadAllText(fileLocation);
                fileSelections = JsonConvert.DeserializeObject<SelectionsWrapper>(fileContents).selections;
                Console.WriteLine(fileSelections.ToString());
            } else {
                fileSelections = new List<FileSelection>();
            }
        }

        public int remove(FileSelection fileSelection) {
            for(int i = 0; i < fileSelections.Count; i++) {
                if(Object.ReferenceEquals(fileSelections.ElementAt(i), fileSelection)) {
                    fileSelections.RemoveAt(i);
                    return i;
                }
            }

            return -1;
        }

        
    }
}
