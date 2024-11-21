using System.Diagnostics;
using static MultiOpenBrowser.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.WebBrowsers
{
    internal class CustomizeBrowser(WebEnvironment webEnvironment) : Chrome(webEnvironment)
    {
        public override StartResult Start(StartOption startOption)
        {
            if (string.IsNullOrWhiteSpace(_webEnvironment.WebBrowser.ExePath))
            {
                throw new ArgumentNullException(nameof(_webEnvironment.WebBrowser.ExePath));
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
