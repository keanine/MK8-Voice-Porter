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
using System.Windows.Shapes;

namespace MK8VoicePorter.Windows
{
    /// <summary>
    /// Interaction logic for ExtractParamsWizard.xaml
    /// </summary>
    public partial class ExtractParamsWizard : Window
    {
        public ExtractParamsWizard()
        {
            InitializeComponent();
        }

        private void btn_ExtractParams_Click(object sender, RoutedEventArgs e)
        {
            lbl_ConsoleOutput.Text = "Extracting. Please do not close this window...";

            string baseContentPath = txt_baseContentFolder.Text;
            string updateContentPath = txt_updateContentFolder.Text;

            if (Directory.Exists(baseContentPath + "/audio/driver/"))
            {
                ExtractParams(baseContentPath);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("The specified Mario Kart 8 content folder could not be found. It will be skipped.", "Error", MessageBoxButton.OK);
            }

            if (Directory.Exists(updateContentPath + "/audio/driver/"))
            {
                ExtractParams(updateContentPath);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("The specified Mario Kart 8 Update content folder could not be found. It will be skipped.", "Error", MessageBoxButton.OK);
            }

            lbl_ConsoleOutput.Text = "Extraction Complete! You may now close this window";
        }

        private void ExtractParams(string path)
        {
            GlobalDirectory.ClearTempFolders();

            string driverPath = path + "/audio/driver/";
            string driverMenuPath = path + "/audio/driver_menu/";
            string driverOpenPath = path + "/audio/driver_open/";

            ExtractBINfromBARS(driverPath, GlobalDirectory.driverParamsDirectory);
            ExtractBINfromBARS(driverMenuPath, GlobalDirectory.menuParamsDirectory);
            ExtractBINfromBARS(driverOpenPath, GlobalDirectory.unlockParamsDirectory);
        }

        private void ExtractBINfromBARS(string barsDirectory, string outputDirectory)
        {
            foreach (string barsFile in Directory.GetFiles(barsDirectory, "*.bars"))
            {
                if (Uwizard.SARC.extract(barsFile, GlobalDirectory.bfwavTempFolder))
                {
                    string driverName = GetDriverNameFromBARS(barsFile);
                    string destination = outputDirectory + driverName + "_param.bin";

                    if (File.Exists(destination))
                    {
                        File.Delete(destination);
                    }

                    File.Move(GlobalDirectory.bfwavTempFolder + "_param.bin", destination);
                    GlobalDirectory.ClearTempFolders();
                }
            }
        }

        private string GetDriverNameFromBARS(string driverFilepath)
        {
            string driverName = System.IO.Path.GetFileNameWithoutExtension(driverFilepath);

            if (driverName.StartsWith("SNDG_M_", StringComparison.OrdinalIgnoreCase) ||
                driverName.StartsWith("SNDG_N_", StringComparison.OrdinalIgnoreCase))
            {
                driverName = driverName.Remove(0, 7);
            }
            else if (driverName.StartsWith("SNDG_", StringComparison.OrdinalIgnoreCase))
            {
                driverName = driverName.Remove(0, 5);
            }

            return driverName;
        }

        /*
                string barsFile = Path.GetFileNameWithoutExtension(barsFilepath);
                string characterName = string.Empty;

                if (Utilities.StringStartsWithAny(barsFile, "MenuDriver_", "SNDG_M_"))
                {
                    characterName = barsFile.Remove(0, 7);
                }
                else if (Utilities.StringStartsWithAny(barsFile, "OpenDriver_", "SNDG_N_"))
                {
                    characterName = barsFile.Remove(0, 7);
                }
                else if (Utilities.StringStartsWithAny(barsFile, "Driver_", "SNDG_"))
                {
                    characterName = barsFile.Remove(0, 5);
                }
        */

        //private void list_BarsFiles_Drop(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        // Note that you can have more than one file.
        //        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        //        foreach (string file in files)
        //        {
        //            if (file.EndsWith(".bars"))
        //            {
        //                list_BarsFiles.Items.Add(file);
        //            }
        //        }

        //        // Assuming you have one file that you care about, pass it off to whatever
        //        // handling code you have defined.
        //        //HandleFileOpen(files[0]);
        //    }
        //}
    }
}
