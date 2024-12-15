using System.Diagnostics;
using System.Text;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    internal class Browser360(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        public override string? ExePath => _webEnvironment.WebBrowser.ExePath ?? GlobalData.Option.Browser360Path;

        public override string? GetStartupArguments(StartOption startOption)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowserDataPath))
            {
                sb.Append($"--user-data-dir=\"{_webEnvironment.WebBrowserDataPath}\" ");
            }
            sb.Append("--no-first-run ");
            sb.Append("--no-default-browser-check ");
            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ProxyServer))
            {
                sb.Append($"--proxy-server=\"{_webEnvironment.WebBrowser.ProxyServer}\" ");
            }
            // 360浏览器特有的一些参数
            sb.Append("--360browser ");
            sb.Append("--no-sandbox ");

            if (_webEnvironment.WebBrowser.DisableWebSecurity)
            {
                sb.Append("--disable-web-security ");
            }
            if (startOption.IncognitoMode == true)
            {
                sb.Append("--incognito "); // 360也支持隐身模式
            }
            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.UserAgent))
            {
                sb.Append($"--user-agent=\"{_webEnvironment.WebBrowser.UserAgent}\" ");
            }
            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.Arguments))
            {
                sb.Append($"{_webEnvironment.WebBrowser.Arguments} ");
            }

            return sb.ToString();
        }

        public override string? GetStartupCmd(StartOption startOption)
        {
            var exePath = _webEnvironment.WebBrowser.ExePath ?? GlobalData.Option.Browser360Path; // 需要在GlobalData中添加Browser360Path
            var aguments = GetStartupArguments(startOption);
            return $"{exePath} {aguments}";
        }

        public override StartResult Start(StartOption startOption)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = _webEnvironment.WebBrowser.ExePath ?? GlobalData.Option.Browser360Path,
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