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
using System.Windows.Shapes;

namespace TextEditorWPF
{
    /// <summary>
    /// Логика взаимодействия для Font.xaml
    /// </summary>
    public partial class Font : Window
    {
        public string actualFontFamily = Properties.Settings.Default.fontFamily;
        public string actualFontStyle = Properties.Settings.Default.fontStyle;
        public string actualFontWeight = Properties.Settings.Default.fontWeight;
        public double actualFontSize = Properties.Settings.Default.fontSize;
        public Font()
        {
            InitializeComponent();
            RandomSample();
            FontChooser.Text = actualFontFamily;
            SizeChooser.Text = actualFontSize.ToString();

            StyleChooser.Text = (actualFontStyle + " " + actualFontWeight).Replace("Normal", "").Trim();
            if(StyleChooser.Text == "")
            {
                StyleChooser.Text = "Regular";
            }
        }

        private void RandomSample()
        {
            string pangram = "";
            switch (new Random().Next(0,4))
            {
                case 0: pangram = "Jackdaws love my big sphinx of quartz."; break;
                case 1: pangram = "The five boxing wizards jump quickly.."; break;
                case 2: pangram = "Pack my box with five dozen liquor jugs."; break;
                case 3: pangram = "Cozy sphinx waves quart jug of bad milk."; break;
                case 4: pangram = "The jay, pig, fox, zebra and my wolves quack!"; break;
            }
            TitleSample.Content = pangram;

            TitleSample.FontFamily = new FontFamily(actualFontFamily);
            switch (actualFontStyle)
            {
                case "Normal": TitleSample.FontStyle = FontStyles.Normal; break;
                case "Italic": TitleSample.FontStyle = FontStyles.Italic; break;
            }
            switch (actualFontStyle)
            {
                case "Normal": TitleSample.FontWeight = FontWeights.Normal; break;
                case "Bold": TitleSample.FontWeight = FontWeights.Bold; break;
            }
            TitleSample.FontSize = actualFontSize;
        }

        private void FontChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TitleSample.FontFamily = new FontFamily(FontChooser.SelectedItem.ToString());
        }

        private void StyleChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (StyleChooser.SelectedIndex)
            {
                case 0: TitleSample.FontStyle = FontStyles.Normal; TitleSample.FontWeight = FontWeights.Normal; break;
                case 1: TitleSample.FontStyle = FontStyles.Italic; TitleSample.FontWeight = FontWeights.Normal; break;
                case 2: TitleSample.FontStyle = FontStyles.Normal; TitleSample.FontWeight = FontWeights.Bold; break;
                case 3: TitleSample.FontStyle = FontStyles.Italic; TitleSample.FontWeight = FontWeights.Bold; break;
                default: break;
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Focus();
        }

        private void SizeChooser_LostFocus(object sender, RoutedEventArgs e)
        {
                double selectedFontSize;
                if (SizeChooser.Text == "" || !double.TryParse(SizeChooser.Text, out selectedFontSize))
                    SizeChooser.SelectedItem = actualFontSize;
                else
                    TitleSample.FontSize = selectedFontSize;
        }
    }
}
