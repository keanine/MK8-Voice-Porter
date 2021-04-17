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

namespace MK8VoicePorter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataGenerationWindow dataGenerationWindow;

        private VoicePorter voicePorter;


        public MainWindow()
        {
            InitializeComponent();
            voicePorter = new VoicePorter(this);
        }

        private void btn_Port_Click(object sender, RoutedEventArgs e)
        {
            voicePorter.Port();
            textBlock_ConsoleOutput.Text = "Complete!";
        }

        private void btn_DataGeneration_Click(object sender, RoutedEventArgs e)
        {
            dataGenerationWindow = new DataGenerationWindow(this, voicePorter);
            dataGenerationWindow.Show();
        }
    }
}
