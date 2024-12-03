using System.Diagnostics;
using System.Text;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    internal class Firefox(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        public override string? GetStartupArguments(StartOption startOption)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowserDataPath))
            {
                sb.Append($"-profile \"{_webEnvironment.WebBrowserDataPath}\" ");
            }
            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ProxyServer))
            {
                // 似乎不支持
            }
            if (_webEnvironment.WebBrowser.DisableWebSecurity)
            {
                // 似乎不支持
            }
            if (startOption.IncognitoMode == true)
            {
                sb.Append("-private ");
            }
            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.UserAgent))
            {
                // 似乎不支持
            }
            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.Arguments))
            {
                sb.Append($"{_webEnvironment.WebBrowser.Arguments} ");
            }

            return sb.ToString();
        }

        public override string? GetStartupCmd(StartOption startOption)
        {
            var exePath = _webEnvironment.WebBrowser.ExePath ?? GlobalData.Option.FirefoxPath;
            var aguments = GetStartupArguments(startOption);
            return $"{exePath} {aguments}";
        }

        public override StartResult Start(StartOption startOption)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = _webEnvironment.WebBrowser.ExePath ?? GlobalData.Option.FirefoxPath,
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
