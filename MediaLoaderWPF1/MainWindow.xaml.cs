using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using MediaLoaderWPF1.model;


namespace MediaLoaderWPF1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private UserFileSelections userFileSelections;

        public MainWindow() {
            InitializeComponent();

            userFileSelections = loadFileSelections();
            addFileSelectionRows();
        }

        public void saveUserSelections() {
            userFileSelections.saveToFile();
        }

        public void onFileSelectionRemoved(FileSelection fileSelection) {

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

            userFileSelections.saveToFile();
        }

        private UserFileSelections loadFileSelections() {
            UserFileSelections userFileSelectios = new UserFileSelections();

            userFileSelectios.loadFromFile();


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
    }

}
