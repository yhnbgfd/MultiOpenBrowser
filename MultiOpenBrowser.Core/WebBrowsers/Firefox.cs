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
                sb.Append($"--user-data-dir=\"{_webEnvironment.WebBrowserDataPath}\" ");
            }
            sb.Append("--no-first-run ");
            sb.Append("--no-default-browser-check ");
            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ProxyServer))
            {
                sb.Append($"--proxy-server=\"{_webEnvironment.WebBrowser.ProxyServer}\" ");
            }
            sb.Append("--restore-last-session ");
            sb.Append("--hide-crash-restore-bubble ");
            sb.Append("--flag-switches-begin ");
            sb.Append("--flag-switches-end ");
            if (_webEnvironment.WebBrowser.DisableWebSecurity)
            {
                sb.Append("--disable-web-security ");//可解决跨域报错
            }
            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.Arguments))
            {
                sb.Append($"{_webEnvironment.WebBrowser.Arguments} ");
            }
            if (startOption.IncognitoMode == true)
            {
                sb.Append("--incognito ");
            }
            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.UserAgent))
            {
                sb.Append($"--user-agent=\"{_webEnvironment.WebBrowser.UserAgent}\" ");
            }

            return sb.ToString();
        }

        public override string? GetStartupCmd(StartOption startOption)
        {
            var exePath = _webEnvironment.WebBrowser.ExePath ?? GlobalData.FirefoxPath;
            var aguments = GetStartupArguments(startOption);
            return $"{exePath} {aguments}";
        }

        public override StartResult Start(StartOption startOption)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = _webEnvironment.WebBrowser.ExePath ?? GlobalData.FirefoxPath,
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
