using System.Windows;
using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebView2BrowserUserControl : UserControl
    {
        public WebView2BrowserUserControl()
        {
            InitializeComponent();
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(addressBar.Text);
            }
        }
    }
}
