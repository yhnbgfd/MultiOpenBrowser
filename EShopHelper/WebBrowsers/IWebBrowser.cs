namespace EShopHelper.WebBrowsers
{
    internal interface IWebBrowser
    {
        /// <summary>
        /// 启动浏览器
        /// </summary>
        StartResult Start(StartOption startOption);

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

        public class StartResult
        {
            public bool IsSuccess { get; set; }
            public int? ProcessId { get; set; }

            public static StartResult SuccessResult(int? processId = null)
            {
                return new StartResult()
                {
                    IsSuccess = true,
                    ProcessId = processId,
                };
            }
        }
    }
}
