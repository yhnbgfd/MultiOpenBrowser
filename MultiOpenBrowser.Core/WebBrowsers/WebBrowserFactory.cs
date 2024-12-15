using Autofac;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    public class WebBrowserFactory(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        public WebBrowserBase WebBrowserInstance => Global.Container.ResolveKeyed<WebBrowserBase>(_webEnvironment.WebBrowser.Type, new NamedParameter("webEnvironment", _webEnvironment));

        public override string? GetStartupArguments(StartOption startOption)
        {
            var startResult = WebBrowserInstance.GetStartupArguments(startOption);
            return startResult;
        }

        public override string? GetStartupCmd(StartOption startOption)
        {
            string? exePath = WebBrowserInstance.ExePath;
            var aguments = WebBrowserInstance.GetStartupArguments(startOption);
            return $"{exePath} {aguments}";
        }

        public override StartResult Start(StartOption startOption)
        {
            var startResult = WebBrowserInstance.Start(startOption);
            if (startResult.IsSuccess == true)
            {
                _webEnvironment.ProcessId = startResult.ProcessId;
            }
            return startResult;
        }
    }
}
