using Autofac;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    public class WebBrowserFactory(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        public override string? GetStartupArguments(StartOption startOption)
        {
            string? startResult;

            var browser = Global.Container.ResolveKeyed<WebBrowserBase>(_webEnvironment.WebBrowser.Type, new NamedParameter("webEnvironment", _webEnvironment));
            startResult = browser.GetStartupArguments(startOption);

            return startResult;
        }

        public override string? GetStartupCmd(StartOption startOption)
        {
            var browser = Global.Container.ResolveKeyed<WebBrowserBase>(_webEnvironment.WebBrowser.Type, new NamedParameter("webEnvironment", _webEnvironment));
            var aguments = browser.GetStartupArguments(startOption);

            string? exePath = browser.ExePath;

            return $"{exePath} {aguments}";
        }

        public override StartResult Start(StartOption startOption)
        {
            StartResult startResult;

            var browser = Global.Container.ResolveKeyed<WebBrowserBase>(_webEnvironment.WebBrowser.Type, new NamedParameter("webEnvironment", _webEnvironment));
            startResult = browser.Start(startOption);

            if (startResult.IsSuccess == true)
            {
                _webEnvironment.ProcessId = startResult.ProcessId;
            }

            return startResult;
        }
    }
}
