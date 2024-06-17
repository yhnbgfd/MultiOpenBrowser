namespace EShopHelper.WebBrowsers
{
    internal abstract class WebBrowserBase : IWebBrowser
    {
        protected WebBrowser _webBrowser;

        public WebBrowserBase(WebBrowser webBrowser)
        {
            _webBrowser = webBrowser;
        }

        public abstract void Start(string? userDataDir, bool incognito = false);
    }
}
