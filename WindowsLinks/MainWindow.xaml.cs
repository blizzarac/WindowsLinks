using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WindowsLinks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;

            var args = Environment.GetCommandLineArgs();
            if (args.Count() != 2)
            {
                throw new Exception("No file parameter given");
            }

            string outputPath = "";
            var fileFolder = args[1];
            bool isDirectory = Directory.Exists(fileFolder);

            if(new FileInfo(fileFolder).Exists)
            {
                var folderBrowser = new FolderBrowserDialog();
                if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    outputPath = folderBrowser.SelectedPath;
                }
            }
            else
            {
                throw new Exception("Parameter file/folder does not exist");
            }

            CreateLink(fileFolder, outputPath, isDirectory);
        }

        private static void CreateLink(string location, string destination, bool isDirectory)
        {
            
            var startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "mklink.exe";
            if (isDirectory)
            {
                startInfo.Arguments = "/D " + location + " " + destination;
            }
            else
            {
                startInfo.Arguments = location + " " + destination;
            }

            var process = new Process {StartInfo = startInfo};
            process.Start();
        }
    }
}
