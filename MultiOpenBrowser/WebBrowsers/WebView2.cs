using MultiOpenBrowser.Views.Windows;
using static MultiOpenBrowser.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.WebBrowsers
{
    internal class WebView2 : WebBrowserBase
    {
        public WebView2(WebEnvironment webEnvironment) : base(webEnvironment)
        {
        }

        public override string? GetArguments(StartOption startOption)
        {
            return null;
        }

        public override StartResult Start(StartOption startOption)
        {
            WebView2BrowserWindow webView2 = new WebView2BrowserWindow(_webEnvironment);
            webView2.Show();

            return StartResult.SuccessResult();
        }
    }
}
