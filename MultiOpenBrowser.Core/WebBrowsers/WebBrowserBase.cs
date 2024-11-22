using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    public abstract class WebBrowserBase(WebEnvironment webEnvironment) : IWebBrowser
    {
        protected WebEnvironment _webEnvironment = webEnvironment;

        public abstract string? GetStartupArguments(StartOption startOption);
        public abstract StartResult Start(StartOption startOption);
    }
}
