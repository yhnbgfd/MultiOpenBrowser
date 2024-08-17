using static MultiOpenBrowser.Entitys.WebBrowser;
using static MultiOpenBrowser.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.WebBrowsers
{
    internal class WebBrowserFactory
    {
        public static (TypeEnum? type, string? arguments) GetArguments(WebEnvironment? webEnvironment, StartOption startOption)
        {
            if (webEnvironment == null)
            {
                return (null, null);
            }

            string? startResult;

            if (webEnvironment.WebBrowser.Type == TypeEnum.MsEdge)
            {
                MsEdge msEdge = new(webEnvironment);
                startResult = msEdge.GetStartupArguments(startOption);
            }
            else if (webEnvironment.WebBrowser.Type == TypeEnum.WebView2)
            {
                WebView2 webView2 = new(webEnvironment);
                startResult = webView2.GetStartupArguments(startOption);
            }
            else
            {
                Chrome chrome = new(webEnvironment);
                startResult = chrome.GetStartupArguments(startOption);
            }

            return (webEnvironment.WebBrowser.Type, startResult);
        }

        public static void Start(WebEnvironment? webEnvironment, StartOption startOption)
        {
            if (webEnvironment == null)
            {
                return;
            }

            StartResult startResult;

            if (webEnvironment.WebBrowser.Type == TypeEnum.MsEdge)
            {
                MsEdge msEdge = new(webEnvironment);
                startResult = msEdge.Start(startOption);
            }
            else if (webEnvironment.WebBrowser.Type == TypeEnum.WebView2)
            {
                WebView2 webView2 = new(webEnvironment);
                startResult = webView2.Start(startOption);
            }
            else
            {
                Chrome chrome = new(webEnvironment);
                startResult = chrome.Start(startOption);
            }

            if (startResult.IsSuccess == true)
            {
                webEnvironment.ProcessId = startResult.ProcessId;
            }
        }
    }
}
