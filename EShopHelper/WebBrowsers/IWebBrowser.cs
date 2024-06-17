namespace EShopHelper.WebBrowsers
{
    internal interface IWebBrowser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDataDir"></param>
        /// <param name="incognito">无痕模式</param>
        void Start(string? userDataDir, bool incognito = false);
    }
}
