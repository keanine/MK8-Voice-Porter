using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace MK8VoicePorter.Windows
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
            //Converter.ConvertBFWAVtoWAV(Strings.bfwavDirectoryU, Strings.wavDirectoryU);
            Converter.ExtractAllUBARS();
        }

        private void btn_ConvertDXwavs_Click(object sender, RoutedEventArgs e)
        {
            //Converter.ConvertBFWAVtoWAV(Strings.bfwavDirectoryDX, Strings.wavDirectoryDX);
            int barsCount = Directory.GetFiles(GlobalDirectory.barsDirectoryDX, "*.bars").Length;

            BackgroundWorker generateFileInfoWorker = new BackgroundWorker();
            generateFileInfoWorker.WorkerReportsProgress = true;
            generateFileInfoWorker.DoWork += ExtractBarsWorkerDX_DoWork;
            generateFileInfoWorker.RunWorkerAsync();

            ProgressWindow progressWindow = new ProgressWindow();
            progressWindow.Owner = Window.GetWindow(this) as MainWindow;
            progressWindow.progressBar.Minimum = 0;
            progressWindow.progressBar.Maximum = barsCount;
            progressWindow.ShowDialog();
        }

        private void btn_GenerateUChecksums_Click(object sender, RoutedEventArgs e)
        {
            int driverCount = Directory.GetDirectories(GlobalDirectory.wavDirectoryU).Length;

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
            int driverCount = Directory.GetDirectories(GlobalDirectory.wavDirectoryDX).Length;

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

        void GenerateFileInfoWorkerU_DoWork(object sender, DoWorkEventArgs e)
        {
            FileInfoGenerator.GenerateFileInfo(GlobalDirectory.wavDirectoryU, GlobalDirectory.bfwavDirectoryU, GlobalDirectory.fileInfoDirectoryU);
        }

        void GenerateFileInfoWorkerDX_DoWork(object sender, DoWorkEventArgs e)
        {
            FileInfoGenerator.GenerateFileInfo(GlobalDirectory.wavDirectoryDX, GlobalDirectory.bfwavDirectoryDX, GlobalDirectory.fileInfoDirectoryDX);
        }

        void ExtractBarsWorkerDX_DoWork(object sender, DoWorkEventArgs e)
        {
            Converter.ExtractDXBARS();
        }

        private void btn_OpenAudioFolder_Click(object sender, RoutedEventArgs e)
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "data/audio/bars/");
            System.Diagnostics.Process.Start(directory);
        }

        private void btn_GenerateJSON_Click(object sender, RoutedEventArgs e)
        {
            DriverIdentityGenerator.GenerateDriverIdentityData();




            //string json = File.ReadAllText(GlobalDirectory.fileInfoDirectoryDX + "Link.json");
            //FileInfoData data = Newtonsoft.Json.JsonConvert.DeserializeObject<FileInfoData>(json);

            //string output = string.Empty;

            //foreach (var element in data.elements)
            //{
            //    output += $"(\"\", \"{element.fileName}\"),\n";
            //}

            //File.WriteAllText("tempFiles.txt", output);
        }
    }
}
