using System.Diagnostics;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class AboutWindow : ReactiveWindow<AboutViewModel>
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public AboutWindow()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                ViewModel = new AboutViewModel();
                this.Bind(ViewModel, vm => vm.AppVersion, v => v.TextBlock_AppVersion.Text).DisposeWith(disposables);
            });
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
