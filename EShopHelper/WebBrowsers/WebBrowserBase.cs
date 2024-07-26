using static EShopHelper.WebBrowsers.IWebBrowser;

namespace EShopHelper.WebBrowsers
{
    internal abstract class WebBrowserBase : IWebBrowser
    {
        protected WebEnvironment _webEnvironment;

        public WebBrowserBase(WebEnvironment webEnvironment)
        {
            _webEnvironment = webEnvironment;
        }

        public abstract StartResult Start(StartOption startOption);
    }
}
