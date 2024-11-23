using System.Diagnostics;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    internal class CustomizeBrowser(WebEnvironment webEnvironment) : Chrome(webEnvironment)
    {
        public override string? GetStartupCmd(StartOption startOption)
        {
            var exePath = _webEnvironment.WebBrowser.ExePath;
            var aguments = GetStartupArguments(startOption);
            return $"{exePath} {aguments}";
        }

        public override StartResult Start(StartOption startOption)
        {
            if (string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ExePath))
            {
                throw new Exception("Need ExePath");
            }

            ProcessStartInfo processStartInfo = new()
            {
                FileName = _webEnvironment.WebBrowser.ExePath,
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
