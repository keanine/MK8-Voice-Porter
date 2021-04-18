using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MK8VoicePorter
{
    /// <summary>
    /// Interaction logic for DataGenerationWindow.xaml
    /// </summary>
    public partial class DataGenerationWindow : Window
    {
        private MainWindow window;

        public DataGenerationWindow(MainWindow window)
        {
            InitializeComponent();
            this.window = window;
        }

        private void btn_ConvertUwavs_Click(object sender, RoutedEventArgs e)
        {
            Converter.ConvertBFWAVtoWAV(Strings.bfwavDirectoryU, Textbox_LAC_Root.Text + "/");
        }

        private void btn_ConvertDXwavs_Click(object sender, RoutedEventArgs e)
        {
            Converter.ConvertBFWAVtoWAV(Strings.bfwavDirectoryDX, Textbox_LAC_Root.Text + "/");
        }

        private void btn_GenerateUChecksums_Click(object sender, RoutedEventArgs e)
        {
            int driverCount = Directory.GetDirectories(Strings.wavDirectoryU).Length;

            BackgroundWorker generateFileInfoWorker = new BackgroundWorker();
            generateFileInfoWorker.WorkerReportsProgress = true;
            generateFileInfoWorker.DoWork += GenerateFileInfoWorkerU_DoWork;
            generateFileInfoWorker.RunWorkerAsync();

            ProgressWindow progressWindow = new ProgressWindow();
            progressWindow.Owner = Window.GetWindow(this) as MainWindow;
            progressWindow.progressBar.Minimum = 0;
            progressWindow.progressBar.Maximum = driverCount;
            progressWindow.ShowDialog();
        }

        private void btn_GenerateDXChecksums_Click(object sender, RoutedEventArgs e)
        {
            int driverCount = Directory.GetDirectories(Strings.wavDirectoryDX).Length;

            BackgroundWorker generateFileInfoWorker = new BackgroundWorker();
            generateFileInfoWorker.WorkerReportsProgress = true;
            generateFileInfoWorker.DoWork += GenerateFileInfoWorkerDX_DoWork;
            generateFileInfoWorker.RunWorkerAsync();

            ProgressWindow progressWindow = new ProgressWindow();
            progressWindow.Owner = Window.GetWindow(this) as MainWindow;
            progressWindow.progressBar.Minimum = 0;
            progressWindow.progressBar.Maximum = driverCount;
            progressWindow.ShowDialog();
        }

        private void btn_LAC_Browse_Click(object sender, RoutedEventArgs e)
        {

        }

        void GenerateFileInfoWorkerU_DoWork(object sender, DoWorkEventArgs e)
        {
            FileInfoGenerator.GenerateFileInfo(Strings.wavDirectoryU, Strings.bfwavDirectoryU, Strings.fileInfoDirectoryU);
        }

        void GenerateFileInfoWorkerDX_DoWork(object sender, DoWorkEventArgs e)
        {
            FileInfoGenerator.GenerateFileInfo(Strings.wavDirectoryDX, Strings.bfwavDirectoryDX, Strings.fileInfoDirectoryDX);
        }
    }
}
