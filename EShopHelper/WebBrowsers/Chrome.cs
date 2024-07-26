using System.Diagnostics;
using System.Text;
using static EShopHelper.WebBrowsers.IWebBrowser;

namespace EShopHelper.WebBrowsers
{
    internal class Chrome : WebBrowserBase
    {
        public Chrome(WebEnvironment webEnvironment) : base(webEnvironment)
        {
        }

        public override int Start(StartOption startOption)
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

            ProcessStartInfo processStartInfo = new()
            {
                FileName = GlobalData.ChromePath,
                Arguments = sb.ToString(),
            };
            Process process = new()
            {
                StartInfo = processStartInfo,
            };
            process.Start();

            return process.Id;
        }
    }
}
