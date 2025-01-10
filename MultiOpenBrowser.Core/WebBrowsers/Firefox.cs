using System.Diagnostics;
using System.Text;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    internal class Firefox(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        protected override string ArgumentPrefix => "-";
        public override string Icon => "Firefox.png";
        public override string? ExePath => string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ExePath) ? GlobalData.Option.FirefoxPath : _webEnvironment.WebBrowser.ExePath;

        public override string? GetStartupArguments(StartOption startOption)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowserDataPath))
            {
                AppendArgument(sb, "profile", _webEnvironment.WebBrowserDataPath);
            }

            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ProxyServer))
            {
                AppendArgument(sb, "proxy-server", _webEnvironment.WebBrowser.ProxyServer);
            }

            if (startOption.IncognitoMode == true)
            {
                AppendArgument(sb, "private-window");
            }

            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.UserAgent))
            {
                // Firefox 通过 about:config 或扩展来修改 UserAgent
            }

            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.Arguments))
            {
                sb.Append($"{_webEnvironment.WebBrowser.Arguments} ");
            }

            return sb.ToString();
        }

        public override string? GetStartupCmd(StartOption startOption)
        {
            var aguments = GetStartupArguments(startOption);
            return $"{ExePath} {aguments}";
        }

        public override StartResult Start(StartOption startOption)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = ExePath,
                Arguments = GetStartupArguments(startOption),
            };
            Process process = new()
            {
                StartInfo = processStartInfo,
            };
            process.Start();

            return StartResult.SuccessResult(process.Id);
        }
    }
}
