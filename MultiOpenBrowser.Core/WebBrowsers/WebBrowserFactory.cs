using Autofac;
using static MultiOpenBrowser.Core.Entitys.WebBrowser;
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

            string? exePath;
            if (_webEnvironment.WebBrowser.Type == TypeEnum.MsEdge)
            {
                exePath = _webEnvironment.WebBrowser.ExePath ?? GlobalData.Option.MsEdgePath;
            }
            else if (_webEnvironment.WebBrowser.Type == TypeEnum.Firefox)
            {
                exePath = _webEnvironment.WebBrowser.ExePath ?? GlobalData.Option.FirefoxPath;
            }
            else if (_webEnvironment.WebBrowser.Type == TypeEnum.Other)
            {
                exePath = _webEnvironment.WebBrowser.ExePath;
            }
            else
            {
                exePath = _webEnvironment.WebBrowser.ExePath ?? GlobalData.Option.ChromePath;
            }

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
