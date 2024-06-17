namespace EShopHelper.WebBrowsers
{
    internal interface IWebBrowser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incognito">无痕模式</param>
        void Start(bool incognito = false);
    }
}
