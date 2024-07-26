namespace EShopHelper.WebBrowsers
{
    internal interface IWebBrowser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incognito">无痕模式</param>
        int Start(bool incognito = false);
    }
}
