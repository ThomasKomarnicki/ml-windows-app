using System;
using System.Windows;

public class DirectoryRow : WrapPanel
{

    private String directoryLocation;

	public DirectoryRow(int width, String directoryLocation)
	{
        this.directoryLocation = directoryLocation;

        Orientation = Orientation.Horizontal;
        HorizontalAlignment = HorizontalAlignment.Left;
        VerticalAlignment = VerticalAlignment.Top;
        Width = width;

        Label label = new Label(directoryLocation);
        label.Width = width - 60;
        Children.Add(label);

        CheckBox subDirectoriesCheckBox = new CheckBox();
        subDirectoriesCheckBox.IsChecked = false;
        subDirectoriesCheckBox.Width = 60;
        subDirectoriesCheckBox.Click += new RoutedEventHandler(subDirectoryCheckbox_Click);
        Children.Add(subDirectoriesCheckBox);
    }

    void subDirectoryCheckbox_Click(object sender, RoutedEventArgs e) {

        Console.Write("checkbox clicked");
    }
}
