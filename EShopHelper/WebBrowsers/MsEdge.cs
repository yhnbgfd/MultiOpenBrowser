using System.Diagnostics;
using System.Text;

namespace EShopHelper.WebBrowsers
{
    internal class MsEdge : WebBrowserBase
    {
        public MsEdge(WebEnvironment webEnvironment) : base(webEnvironment)
        {
        }

        public override void Start(bool incognito = false)
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
            if (incognito == true)
            {
                sb.Append("--inprivate ");
            }

            ProcessStartInfo processStartInfo = new()
            {
                FileName = GlobalData.MsEdgePath,
                Arguments = sb.ToString(),
            };
            Process process = new()
            {
                StartInfo = processStartInfo,
            };
            process.Start();
        }
    }
}
