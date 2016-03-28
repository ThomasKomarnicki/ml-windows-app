using System;
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
using NancyML.model;

namespace MediaLoaderWPF1 {
    /// <summary>
    /// Interaction logic for DirectoryRowControl.xaml
    /// </summary>
    public partial class DirectoryRowControl : UserControl {

        private MainWindow _mainWindow;
        private FileSelection _fileSelection;

        public DirectoryRowControl() {
            InitializeComponent();
        }

        public DirectoryRowControl(FileSelection fileSelection) {
            InitializeComponent();
            directoryPath.Content = fileSelection.directoryPath;
            includeSubDirs.IsChecked = fileSelection.includeSubDirs;
            _fileSelection = fileSelection;
        }

        public void setMainWindow(MainWindow mainWindow) {
            _mainWindow = mainWindow;
        }

        private void includeSubDirs_Checked(object sender, RoutedEventArgs e) {
            if (_fileSelection != null)
            {
                _fileSelection.includeSubDirs = includeSubDirs.IsChecked.Value;
                _mainWindow.onFileSelectionChanged(_fileSelection);
            }

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e) {
            _mainWindow.onFileSelectionRemoved(_fileSelection);
        }
    }


}
