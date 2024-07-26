namespace EShopHelper.WebBrowsers
{
    internal interface IWebBrowser
    {
        /// <summary>
        /// 启动浏览器
        /// </summary>
        int Start(StartOption startOption);

        /// <summary>
        /// 浏览器启动参数
        /// </summary>
        public class StartOption
        {
            /// <summary>
            /// 无痕模式
            /// </summary>
            public bool IncognitoMode { get; set; } = false;
        }
    }
}
