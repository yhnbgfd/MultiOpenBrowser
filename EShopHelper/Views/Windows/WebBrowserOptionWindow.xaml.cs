using NLog;
using System.Windows;
using System.Windows.Input;

namespace EShopHelper.Views.Windows
{
    public partial class WebBrowserOptionWindow : Window
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebBrowser WebBrowser { get; set; } = WebBrowser.Default;

        public WebBrowserOptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebBrowserRepo webBrowserRepo = new(null);
                await webBrowserRepo.InsertOrUpdateAsync(WebBrowser);

                this.Close();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
