﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace MediaLoaderWPF1.model {
    class UserFileSelections {

        private string fileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "user_file_selections.json");

        List<FileSelection> fileSelections;

        public UserFileSelections() {

        }


        public void saveToFile() {

        }

        public void loadFromFile() {
            if (!File.Exists(fileLocation)) { 
                String fileContents = File.ReadAllText(fileLocation);
                fileSelections = JsonConvert.DeserializeObject<List<FileSelection>>(fileContents);
                Console.WriteLine(fileSelections.ToString());
            }
        }
        
    }
}
