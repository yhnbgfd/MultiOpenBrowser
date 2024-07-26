using static EShopHelper.Entitys.WebBrowser;

namespace EShopHelper.WebBrowsers
{
    internal class WebBrowserFactory
    {
        public static void Start(WebEnvironment? webEnvironment, bool incognito = false)
        {
            if (webEnvironment == null)
            {
                return;
            }

            if (webEnvironment.WebBrowser.Type == TypeEnum.MsEdge)
            {
                MsEdge msEdge = new(webEnvironment);
                webEnvironment.ProcessId = msEdge.Start(incognito);
            }
            else if (webEnvironment.WebBrowser.Type == TypeEnum.WebView2)
            {
                WebView2 webView2 = new(webEnvironment);
                webEnvironment.ProcessId = webView2.Start(incognito);
            }
            else
            {
                Chrome chrome = new(webEnvironment);
                webEnvironment.ProcessId = chrome.Start(incognito);
            }
        }
    }
}
