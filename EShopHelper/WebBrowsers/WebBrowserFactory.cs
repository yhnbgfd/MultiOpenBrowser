using static EShopHelper.Entitys.WebBrowser;
using static EShopHelper.WebBrowsers.IWebBrowser;

namespace EShopHelper.WebBrowsers
{
    internal class WebBrowserFactory
    {
        public static void Start(WebEnvironment? webEnvironment, StartOption startOption)
        {
            if (webEnvironment == null)
            {
                return;
            }

            if (webEnvironment.WebBrowser.Type == TypeEnum.MsEdge)
            {
                MsEdge msEdge = new(webEnvironment);
                webEnvironment.ProcessId = msEdge.Start(startOption);
            }
            else if (webEnvironment.WebBrowser.Type == TypeEnum.WebView2)
            {
                WebView2 webView2 = new(webEnvironment);
                webEnvironment.ProcessId = webView2.Start(startOption);
            }
            else
            {
                Chrome chrome = new(webEnvironment);
                webEnvironment.ProcessId = chrome.Start(startOption);
            }
        }
    }
}
