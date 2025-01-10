using System.Diagnostics;
using System.Text;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    internal class Browser360(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        public override string Icon => "360_browser.png";
        public override string? ExePath => string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ExePath) ? GlobalData.Option.Browser360Path : _webEnvironment.WebBrowser.ExePath;

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