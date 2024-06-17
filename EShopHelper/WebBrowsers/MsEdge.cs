using System.Diagnostics;
using System.Text;

namespace EShopHelper.WebBrowsers
{
    internal class MsEdge : WebBrowserBase
    {
        public MsEdge(WebBrowser webBrowser) : base(webBrowser)
        {
        }

        public override void Start(string? userDataDir, bool incognito = false)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(userDataDir))
            {
                sb.Append($"--user-data-dir=\"{userDataDir}\" ");
            }
            sb.Append("--no-first-run ");
            sb.Append("--no-default-browser-check ");
            if (!string.IsNullOrWhiteSpace(_webBrowser.ProxyServer))
            {
                sb.Append($"--proxy-server=\"{_webBrowser.ProxyServer}\" ");
            }
            sb.Append("--restore-last-session ");
            sb.Append("--hide-crash-restore-bubble ");
            sb.Append("--flag-switches-begin ");
            sb.Append("--flag-switches-end ");
            if (_webBrowser.DisableWebSecurity)
            {
                sb.Append("--disable-web-security ");//可解决跨域报错
            }
            if (!string.IsNullOrWhiteSpace(_webBrowser.Arguments))
            {
                sb.Append($"{_webBrowser.Arguments} ");
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
