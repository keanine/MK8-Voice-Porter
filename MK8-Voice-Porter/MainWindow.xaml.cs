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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_DataGeneration_Click(object sender, RoutedEventArgs e)
        {
            dataGenerationWindow = new DataGenerationWindow(this);
            dataGenerationWindow.Show();
        }
    }
}
