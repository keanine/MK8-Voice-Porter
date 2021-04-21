using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MK8VoicePorter.Windows
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        bool allowClosing = false;

        public ProgressWindow()
        {
            InitializeComponent();
            allowClosing = false;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker progressBarWorker = new BackgroundWorker();
            progressBarWorker.WorkerReportsProgress = true;
            progressBarWorker.DoWork += ProgressBarWorker_DoWork;
            progressBarWorker.ProgressChanged += ProgressBarWorker_ProgressChanged;
            progressBarWorker.RunWorkerAsync();
        }

        void ProgressBarWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                (sender as BackgroundWorker).ReportProgress(Utilities.progressValue);
                Thread.Sleep(100);
            }
        }

        void ProgressBarWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            lbl_description.Content = Utilities.progressDesc;

            if (progressBar.Value == progressBar.Maximum)
            {
                btn_Done.IsEnabled = true;
            }
        }

        private void btn_Done_Click(object sender, RoutedEventArgs e)
        {
            allowClosing = true;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = !allowClosing;

            if (allowClosing)
            {
                Utilities.progressValue = 0;
            }
        }
    }
}
