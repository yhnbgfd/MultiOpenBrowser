using System.Diagnostics;
using System.Text;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    internal class Chrome(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        public override string Icon => "GoogleChrome.png";
        public override string? ExePath => string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ExePath) ? GlobalData.Option.ChromePath : _webEnvironment.WebBrowser.ExePath;

        public override string? GetStartupArguments(StartOption startOption)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowserDataPath))
            {
                AppendArgument(sb, "user-data-dir", _webEnvironment.WebBrowserDataPath);
            }

            AppendArgument(sb, "no-first-run");
            AppendArgument(sb, "no-default-browser-check");

            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ProxyServer))
            {
                AppendArgument(sb, "proxy-server", _webEnvironment.WebBrowser.ProxyServer);
            }

            if (_webEnvironment.WebBrowser.RestoreLastSession)
            {
                AppendArgument(sb, "restore-last-session");
            }
            //AppendArgument(sb, "hide-crash-restore-bubble");

            if (_webEnvironment.WebBrowser.DisableWebSecurity)
            {
                AppendArgument(sb, "disable-web-security");//可解决跨域报错
            }

            if (startOption.IncognitoMode == true)
            {
                AppendArgument(sb, "incognito");
            }

            if (!string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.UserAgent))
            {
                AppendArgument(sb, "user-agent", _webEnvironment.WebBrowser.UserAgent);
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
