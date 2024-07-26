namespace EShopHelper.WebBrowsers
{
    internal abstract class WebBrowserBase : IWebBrowser
    {
        protected WebEnvironment _webEnvironment;

        public WebBrowserBase(WebEnvironment webEnvironment)
        {
            _webEnvironment = webEnvironment;
        }

        public abstract int Start(bool incognito = false);
    }
}
