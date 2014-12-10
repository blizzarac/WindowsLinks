using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WindowsLinks
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            if (args.Count() != 1)
            {
                throw new Exception("No file parameter given");
            }

            string outputPath = "";
            var fileFolder = args[0];
            bool isDirectory = Directory.Exists(fileFolder);

            if (isDirectory || new FileInfo(fileFolder).Exists)
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

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd",
                UseShellExecute = false
            };

            var linkName = location.Split('\\').Last().Trim();

            if (isDirectory)
            {
                startInfo.Arguments = "/c mklink /D " + destination + "\\" + linkName + " " + location;
            }
            else
            {
                startInfo.Arguments = "/c mklink " + destination + "\\" + linkName + " " + location;
            }

            var process = new Process { StartInfo = startInfo };

            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                Thread.Sleep(int.MaxValue);
            }
        }
    }
}
