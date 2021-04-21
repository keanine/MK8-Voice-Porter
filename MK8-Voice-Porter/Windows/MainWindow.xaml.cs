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

namespace MK8VoicePorter.Windows
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
        }

        private void btn_DataGeneration_Click(object sender, RoutedEventArgs e)
        {
            dataGenerationWindow = new DataGenerationWindow(this);
            dataGenerationWindow.Show();
        }

        private void btn_Port_Click(object sender, RoutedEventArgs e)
        {
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
        }

        private void cmb_TargetDriver_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            targetDriver = cmb_TargetDriver.SelectedItem.ToString();
        }
    }
}
