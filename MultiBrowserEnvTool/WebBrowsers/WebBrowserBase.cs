using static MultiBrowserEnvTool.WebBrowsers.IWebBrowser;

namespace MultiBrowserEnvTool.WebBrowsers
{
    internal abstract class WebBrowserBase(WebEnvironment webEnvironment) : IWebBrowser
    {
        protected WebEnvironment _webEnvironment = webEnvironment;

        public abstract string? GetStartupArguments(StartOption startOption);
        public abstract StartResult Start(StartOption startOption);
    }
}
