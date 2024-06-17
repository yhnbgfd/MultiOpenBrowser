using EShopHelper.Views.Windows;

namespace EShopHelper.WebBrowsers
{
    internal class WebView2 : WebBrowserBase
    {
        public WebView2(WebEnvironment webEnvironment) : base(webEnvironment)
        {
        }

        public override void Start(bool incognito = false)
        {
            WebView2BrowserWindow webView2 = new WebView2BrowserWindow(_webEnvironment);
            webView2.Show();
        }
    }
}
