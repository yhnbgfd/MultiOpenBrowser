using NLog;
using System.Windows;

namespace EShopHelper.Views.Windows
{
    public partial class WebEnvironmentOptionWindow : Window
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment WebEnvironment { get; set; } = WebEnvironment.Default;
        public WebBrowser? WebBrowser => WebEnvironment.WebBrowser;

        public WebEnvironmentOptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebBrowserRepo webBrowserRepo = new(null);
                WebEnvironment.WebBrowser = await webBrowserRepo.InsertOrUpdateAsync(WebEnvironment.WebBrowser!);

                WebEnvironment.WebBrowserId = WebEnvironment.WebBrowser.Id;

                WebEnvironmentRepo webEnvironmentRepo = new(null);
                await webEnvironmentRepo.InsertOrUpdateAsync(WebEnvironment);

                this.Close();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
