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
                msEdge.Start(incognito);
            }
            else if (webEnvironment.WebBrowser.Type == TypeEnum.WebView2)
            {
                WebView2 webView2 = new(webEnvironment);
                webView2.Start(incognito);
            }
            else
            {
                Chrome chrome = new(webEnvironment);
                chrome.Start(incognito);
            }
        }
    }
}
