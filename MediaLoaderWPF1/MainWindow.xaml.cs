using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using UserSelectionLibrary.model;


namespace MediaLoaderWPF1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly UserFileSelections userFileSelections;

        BackgroundWorker _backgroundWorker = new BackgroundWorker();

        public MainWindow() {
            InitializeComponent();


//            TrayIcon.Icon = Properties.Resources.tray_ico;
            

            userFileSelections = loadFileSelections();
            AddFileSelectionRows();

//            new NetworkDemo();

            ConfigMessage();

        }

        public void ConfigMessage()
        {
            if (userFileSelections.fileSelections.Count > 0) {
                AddFoldersLabel.Visibility = Visibility.Hidden;
            } else {
                AddFoldersLabel.Visibility = Visibility.Visible;
            }
        }

        public void SaveUserSelections() {
            userFileSelections.SaveToFile();
        }

        public void OnFileSelectionRemoved(FileSelection fileSelection) {
            int index = userFileSelections.Remove(fileSelection);
            // remove panel from directoriesPanel at index
            directoriesPanel.Children.RemoveAt(index);
            userFileSelections.SaveToFile();
            ConfigMessage();
        }

        public void OnFileSelectionChanged(FileSelection fileSelection) {
            UpdateModel(fileSelection);

            ConfigMessage();

        }

        private void addDirectoryButton_Click(object sender, RoutedEventArgs e) {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Select Directory";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = "C:/Users";

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = "C:/Users";
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok) {
                var folder = dlg.FileName;

                AddDirectory(folder);
            }
        }

        private void AddDirectory(string directory) {

            if (!userFileSelections.Has(directory))
            {
                Console.Write(@"selected directory " + directory);

                FileSelection fileSelection = new FileSelection(directory);
                userFileSelections.fileSelections.Add(fileSelection);

                DirectoryRowControl control = new DirectoryRowControl(fileSelection);
                control.setMainWindow(this);
                directoriesPanel.Children.Add(control);


                UpdateModel(fileSelection);
                ConfigMessage();
            }

        }

        private UserFileSelections loadFileSelections() {
            UserFileSelections userFileSelectios = new UserFileSelections();

            userFileSelectios.LoadFromFile();


            return userFileSelectios;
        }

        private void AddFileSelectionRows() {
            directoriesPanel.Children.Clear();
            foreach (FileSelection fileSelection in userFileSelections.fileSelections) {
                DirectoryRowControl control = new DirectoryRowControl(fileSelection);
                control.setMainWindow(this);
                directoriesPanel.Children.Add(control);

            }
        }

        private void UpdateModel(FileSelection fileSelection)
        {

            if (!fileSelection.HasAllThumbnails())
            {
                _backgroundWorker = new BackgroundWorker();
                _backgroundWorker.DoWork += delegate
                {
                    fileSelection.CreateThumbnails();
                    SaveUserSelections();
                };
                _backgroundWorker.RunWorkerAsync();
            }
        }

    }

}
