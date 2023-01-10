using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace TextEditorWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Error()
        {
            MessageBox.Show("Unexpected Error Occurred", "Why?", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        private void Saved()
        {
            MenuItemIsSaved.Header = "Saved! :)";
            isSaved = true;
        }

        public MainWindow()
        {
            InitializeComponent();
            MainTextBox.FontFamily = new FontFamily(Properties.Settings.Default.fontFamily);
            MainTextBox.FontWeight = Properties.Settings.Default.fontWeight == "Bold" ? FontWeights.Bold : FontWeights.Regular;
            MainTextBox.FontStyle = Properties.Settings.Default.fontStyle == "Italic" ? FontStyles.Italic : FontStyles.Normal;
            MainTextBox.FontSize = Properties.Settings.Default.fontSize;
        }

        string savedPathToFile = null;
        bool isSaved = true;

        private void NewFileCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTextBox.Text != "" || !isSaved)
            {
                MessageBoxResult userChoice = MessageBox.Show("Save file before creating new one?", "Create new file", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                switch (userChoice)
                {
                    case MessageBoxResult.Yes: SaveCommandBinding_Executed(sender, e); break;
                    case MessageBoxResult.No: break;
                    case MessageBoxResult.Cancel: return;
                }
            }
            savedPathToFile = "";
            MainTextBox.Text = "";
            Title = "New File";
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTextBox.Text != "")
            {
                MessageBoxResult userChoice = MessageBox.Show("Save file before open?", "Open file", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                switch (userChoice)
                {
                    case MessageBoxResult.Yes: SaveCommandBinding_Executed(sender, e); break;
                    case MessageBoxResult.No: break;
                    case MessageBoxResult.Cancel: return;
                }
            }
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "document.txt";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";

            if (dialog.ShowDialog() == true)
                savedPathToFile = dialog.FileName;
            else
                return;
            MainTextBox.Text = File.ReadAllText(savedPathToFile);
            Title = savedPathToFile;
            Saved();
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(savedPathToFile))
                SaveAsCommandBinding_Executed(sender, e);
            else
                File.WriteAllText(savedPathToFile, MainTextBox.Text);

            Saved();
        }

        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "document.txt";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";

            if (dialog.ShowDialog() == false)
                return;
            else
            {
                savedPathToFile = dialog.FileName;
                Title = savedPathToFile;
                File.WriteAllText(dialog.FileName, MainTextBox.Text);
            }
            Saved();
        }



        private void MenuItemFont_Click(object sender, RoutedEventArgs e)
        {
            /*Font customFontWindow = new Font();
            customFontWindow.ShowDialog();*/

            FontDialog dialog = new FontDialog();
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainTextBox.FontFamily = new FontFamily(dialog.Font.Name);
                MainTextBox.FontSize = dialog.Font.Size;
                MainTextBox.FontWeight = dialog.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                MainTextBox.FontStyle = dialog.Font.Italic ? FontStyles.Italic: FontStyles.Normal;
                Properties.Settings.Default.fontFamily = dialog.Font.Name;
                Properties.Settings.Default.fontSize = dialog.Font.Size;
                Properties.Settings.Default.fontWeight = dialog.Font.Bold ? "Bold" : "Regular";
                Properties.Settings.Default.fontStyle = dialog.Font.Italic ? "Italic" : "Normal";
                Properties.Settings.Default.Save();
            }
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isSaved = false;
            MenuItemIsSaved.Header = "Not saved :(";
        }

        private void ColorPickerBackground_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (ColorPickerBackground.SelectedColor != null)
            {
                MainTextBox.Background = new SolidColorBrush(ColorPickerBackground.SelectedColor.Value);
            }
        }

        private void ColorPickerText_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if(ColorPickerText.SelectedColor != null)
            {
                MainTextBox.Foreground = new SolidColorBrush(ColorPickerText.SelectedColor.Value);
            }
        }
    }
}
