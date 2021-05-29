using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MK8VoiceTool.Windows
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            txtblock_information.Inlines.Clear();

            Hyperlink link = new Hyperlink();
            link.Inlines.Add("MK8 Voice Porter");
            link.NavigateUri = new Uri("https://github.com/keanine/MK8-Voice-Porter");
            link.RequestNavigate += Hyperlink_RequestNavigate;

            Bold bold = new Bold();
            bold.Inlines.Add(MainWindow.version);

            txtblock_information.Inlines.Add(link);
            txtblock_information.Inlines.Add(" by Keanen Collins (Keanine)");
            txtblock_information.Inlines.Add(new LineBreak());
            txtblock_information.Inlines.Add(bold);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        private void btn_Help_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/keanine/MK8-Voice-Porter/blob/main/README.md"));
        }
    }
}
