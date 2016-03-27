using System;
using System.Windows;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Dialogs;
using NancyML;
using NancyML.model;


namespace MediaLoaderWPF1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private UserFileSelections userFileSelections;

        private NancyServer _server;

        public MainWindow() {
            InitializeComponent();

            userFileSelections = loadFileSelections();
            addFileSelectionRows();


            _server = new NancyServer();

        }

        public void saveUserSelections() {
            userFileSelections.SaveToFile();
        }

        public void onFileSelectionRemoved(FileSelection fileSelection) {
            int index = userFileSelections.Remove(fileSelection);
            // remove panel from directoriesPanel at index
            directoriesPanel.Children.RemoveAt(index);
            userFileSelections.SaveToFile();
        }

        public void onFileSelectionChanged(FileSelection fileSelection) {
            saveUserSelections();
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
                addDirectory(folder);
            }
        }

        private void addDirectory(String directory) {
            Console.Write("selecte directory "+directory);

            FileSelection fileSelection = new FileSelection(directory, false);
            userFileSelections.fileSelections.Add(fileSelection);

            DirectoryRowControl control = new DirectoryRowControl(fileSelection);
            control.setMainWindow(this);
            directoriesPanel.Children.Add(control);

            userFileSelections.SaveToFile();
        }

        private UserFileSelections loadFileSelections() {
            UserFileSelections userFileSelectios = new UserFileSelections();

            userFileSelectios.LoadFromFile();


            return userFileSelectios;
        }

        private void addFileSelectionRows() {
            directoriesPanel.Children.Clear();
            foreach (FileSelection fileSelection in userFileSelections.fileSelections) {
                DirectoryRowControl control = new DirectoryRowControl(fileSelection);
                control.setMainWindow(this);
                directoriesPanel.Children.Add(control);

            }
        }

        private void startService() {
            ProcessStartInfo startInfo = new ProcessStartInfo("\\nssm\\win64\\nssm.exe", "");

        }
    }

}
