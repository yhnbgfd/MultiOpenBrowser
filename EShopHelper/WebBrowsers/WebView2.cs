using EShopHelper.Views.Windows;
using static EShopHelper.WebBrowsers.IWebBrowser;

namespace EShopHelper.WebBrowsers
{
    internal class WebView2 : WebBrowserBase
    {
        public WebView2(WebEnvironment webEnvironment) : base(webEnvironment)
        {
        }

        public override int Start(StartOption startOption)
        {
            WebView2BrowserWindow webView2 = new WebView2BrowserWindow(_webEnvironment);
            webView2.Show();

            return 0;
        }
    }
}
