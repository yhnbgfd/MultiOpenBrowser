using System.Windows;
using System.Windows.Controls;

namespace MultiOpenBrowser.Views.UserControls
{
    public partial class WebView2BrowserUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebView2BrowserUserControl()
        {
            InitializeComponent();
        }

        private void Button_Go_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (webView != null && webView.CoreWebView2 != null)
                {
                    webView.CoreWebView2.Navigate(addressBar.Text);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
