namespace EShopHelper.WebBrowsers
{
    internal class WebView2 : WebBrowserBase
    {
        public WebView2(WebBrowser webBrowser) : base(webBrowser)
        {
        }

        public override void Start(string? userDataDir, bool incognito = false)
        {
            throw new NotImplementedException();
        }
    }
}
