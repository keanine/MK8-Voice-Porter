using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MK8VoiceTool.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string targetDriver;
        public static string targetFormat;

        private DataGenerationWindow dataGenerationWindow;

        public MainWindow()
        {
            InitializeComponent();
            GlobalDirectory.RegenerateAllDirectories();

            string[] filepaths = Directory.GetFiles(GlobalDirectory.identitiesDirectory);
            foreach (string filepath in filepaths)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(filepath);
                cmb_TargetDriver.Items.Add(name);
            }
            cmb_TargetDriver.SelectedIndex = 22;

            cmb_TargetFormat.Items.Add("BARS");
            cmb_TargetFormat.Items.Add("BFWAV");
            cmb_TargetFormat.Items.Add("BFWAV (User-Friendly)");
            cmb_TargetFormat.Items.Add("WAV (User-Friendly)");
            cmb_TargetFormat.SelectedIndex = 0;

            CheckIfCanPort();
        }

        private void btn_DataGeneration_Click(object sender, RoutedEventArgs e)
        {
            dataGenerationWindow = new DataGenerationWindow(this);
            dataGenerationWindow.Show();
        }

        private void btn_Port_Click(object sender, RoutedEventArgs e)
        {
            if (canPort)
            {
                //if ((VoiceFileFormat)cmb_TargetFormat.SelectedIndex == VoiceFileFormat.BARS)
                //{
                //    MessageBoxResult result = MessageBox.Show($"When exporting to BARS, menu and unlock files are currently unsupported. Continue?", "Info", MessageBoxButton.OKCancel);
                //    if (result == MessageBoxResult.Cancel)
                //    {
                //        return;
                //    }
                //}

                if (Directory.GetFiles(GlobalDirectory.outputFolder).Length > 0)
                {
                    MessageBoxResult result = MessageBox.Show("There are files in the output folder. If you continue these will be deleted", "Warning", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }

                VoicePorter.Port(cmb_TargetDriver.SelectedItem.ToString(), (VoiceFileFormat)cmb_TargetFormat.SelectedIndex);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"No param.bin was found for {cmb_TargetDriver.SelectedItem}. If you wish to export to BARS you must export your params first", "Error", MessageBoxButton.OK);
            }
        }

        private void btn_About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Owner = this;
            about.ShowDialog();
        }

        private void cmb_TargetFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            targetFormat = cmb_TargetFormat.SelectedItem.ToString();
            cmb_TargetDriver.IsEnabled = !(cmb_TargetFormat.SelectedItem.ToString().Contains("User-Friendly"));
            CheckIfCanPort();
        }

        private void cmb_TargetDriver_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            targetDriver = cmb_TargetDriver.SelectedItem.ToString();
            CheckIfCanPort();
        }

        private void btn_InputFolder_Click(object sender, RoutedEventArgs e)
        {
            string directory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "files/input/");
            Process.Start(directory);
        }

        private void btn_OutputFolder_Click(object sender, RoutedEventArgs e)
        {
            string directory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "files/output/");
            Process.Start(directory);
        }

        private bool canPort = false;

        private void CheckIfCanPort()
        {
            if (cmb_TargetDriver.SelectedItem != null)
            {
                string targetDriver = cmb_TargetDriver.SelectedItem.ToString();
                VoiceFileFormat targetFormat = (VoiceFileFormat)cmb_TargetFormat.SelectedIndex;

                if (targetFormat == VoiceFileFormat.BARS)
                {
                    if (File.Exists(GlobalDirectory.driverParamsDirectory + targetDriver + "_param.bin")) // if target has param.bin
                    {
                        canPort = true;
                        btn_Port.Foreground = Brushes.Black;

                        btn_ExtractParams.Visibility = Visibility.Collapsed;
                        Thickness margin = lbl_Message.Margin;
                        margin.Top = 35;
                        lbl_Message.Margin = margin;
                    }
                    else
                    {
                        canPort = false;
                        btn_Port.Foreground = Brushes.Red;

                        btn_ExtractParams.Visibility = Visibility.Visible;
                        Thickness margin = lbl_Message.Margin;
                        margin.Top = 60;
                        lbl_Message.Margin = margin;
                        return;
                    }
                }
            }
        }

        private void btn_ExtractParams_Click(object sender, RoutedEventArgs e)
        {
            ExtractParamsWizard extractParams = new ExtractParamsWizard();
            extractParams.Owner = this;
            extractParams.ShowDialog();

            //Return
            CheckIfCanPort();
        }

        private void btn_WAVtoBFWAV_Click(object sender, RoutedEventArgs e)
        {
            Converter.ConvertBFWAVtoWAV("files/testing/bfwav/", "files/testing/wav/");

            byte[] fileAudioData;
            string fileAudioIdntr;
            BARSViewer.BMETA.STRG fileStrg;
            WAVtoBFWAV.ConvertWAVtoBFWAV("files/testing/wav/test.wav", "files/testing/output/test.bfwav");
        }
    }
}
