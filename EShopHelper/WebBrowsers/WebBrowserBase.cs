namespace EShopHelper.WebBrowsers
{
    internal abstract class WebBrowserBase : IWebBrowser
    {
        protected WebEnvironment _webEnvironment;

        public WebBrowserBase(WebEnvironment webEnvironment)
        {
            _webEnvironment = webEnvironment;
        }

        public abstract void Start(bool incognito = false);
    }
}
