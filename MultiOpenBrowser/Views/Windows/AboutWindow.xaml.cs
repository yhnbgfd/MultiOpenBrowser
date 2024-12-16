using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class AboutWindow : Window
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public string? AppVersion => GlobalData.AppVersion;

        public AboutWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_Url_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var url = this.TextBlock_Url.Text.Trim();
            Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }
    }
}
