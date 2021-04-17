using System;
using System.Collections.Generic;
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

namespace MK8VoicePorter
{
    /// <summary>
    /// Interaction logic for DataGenerationWindow.xaml
    /// </summary>
    public partial class DataGenerationWindow : Window
    {
        private MainWindow window;
        private VoicePorter voicePorter;

        public DataGenerationWindow(MainWindow window, VoicePorter voicePorter)
        {
            InitializeComponent();
            this.window = window;
            this.voicePorter = voicePorter;
        }

        private void btn_GenerateJSON_Click(object sender, RoutedEventArgs e)
        {
            AudioComparison comparison = new AudioComparison();
            comparison.GenerateJSONData(this);
        }
    }
}
