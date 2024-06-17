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

            Start(webEnvironment.WebBrowser, webEnvironment.WebBrowserDataPath, incognito);
        }

        private static void Start(WebBrowser? webBrowser, string? userDataDir, bool incognito = false)
        {
            if (webBrowser == null)
            {
                return;
            }

            if (webBrowser.Type == TypeEnum.MsEdge)
            {
                MsEdge msEdge = new(webBrowser);
                msEdge.Start(userDataDir, incognito);
            }
            else if (webBrowser.Type == TypeEnum.WebView2)
            {
                WebView2 webView2 = new(webBrowser);
                webView2.Start(userDataDir, incognito);
            }
            else
            {
                Chrome chrome = new(webBrowser);
                chrome.Start(userDataDir, incognito);
            }
        }
    }
}
