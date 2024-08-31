using MultiOpenBrowser.Views.Windows;
using static MultiOpenBrowser.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.WebBrowsers
{
    internal class WebView2(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        public override string? GetStartupArguments(StartOption startOption)
        {
            return null;
        }

        public override StartResult Start(StartOption startOption)
        {
            WebView2BrowserWindow webView2 = new(_webEnvironment);
            webView2.Show();

            return StartResult.SuccessResult();
        }
    }
}
