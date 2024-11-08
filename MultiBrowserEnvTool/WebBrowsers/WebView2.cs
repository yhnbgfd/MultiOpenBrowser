#if WINDOWS
using MultiOpenBrowser.Views.Windows;
#endif
using static MultiBrowserEnvTool.WebBrowsers.IWebBrowser;

namespace MultiBrowserEnvTool.WebBrowsers
{
    internal class WebView2(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        public override string? GetStartupArguments(StartOption startOption)
        {
            return null;
        }

        public override StartResult Start(StartOption startOption)
        {
#if WINDOWS
            WebView2BrowserWindow webView2 = new(_webEnvironment);
            webView2.Show();

            return StartResult.SuccessResult();
#else
            return StartResult.FailResult();
#endif
        }
    }
}
