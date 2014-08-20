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
using System.IO;

namespace CompilerConfigurer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string CONFIG_FILE_PATH = "config";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(File.Exists(CONFIG_FILE_PATH))
            {
                var fileContents = File.ReadAllLines(CONFIG_FILE_PATH);
                vbspLocation.Text = TryGetLine(fileContents, 1);
                vbspParameters.Text = TryGetLine(fileContents, 3);
                vradLocation.Text = TryGetLine(fileContents, 5);
                vradParameters.Text = TryGetLine(fileContents, 7);
                vvisLocation.Text = TryGetLine(fileContents, 9);
                vvisParameters.Text = TryGetLine(fileContents, 11);
                bspDirectory.Text = TryGetLine(fileContents, 13);
            }
            SaveButton.IsEnabled = false;
        }

        private string TryGetLine(string[] strings, int line)
        {
            return strings.Length > line ? 
                strings[line] :
                string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllLines(CONFIG_FILE_PATH,new string[]{
                "vbsp.exe Path:",  vbspLocation.Text,
                "vbsp.exe Parameters:", vbspParameters.Text,
                "vrad.exe Path:", vradLocation.Text,
                "vrad.exe Parameters:", vradParameters.Text,
                "vvis.exe Path:", vvisLocation.Text,
                "vvis.exe Parameters:", vvisParameters.Text,
                "Output directory (make sure ends with \\):", bspDirectory.Text,
                "","","", "Don't change the above unless you know what you're doing... These bad boys are hard coded to read from specific lines, so keep the compiler path and parameters right where they are"
            });
            SaveButton.IsEnabled = false;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveButton.IsEnabled = true;
        }
    }
}
