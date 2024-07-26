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

        /// <summary>
        /// 浏览器启动结果
        /// </summary>
        public class StartResult
        {
            /// <summary>
            /// 是否成功
            /// </summary>
            public bool IsSuccess { get; set; }
            /// <summary>
            /// 进程ID
            /// </summary>
            public int? ProcessId { get; set; }

            /// <summary>
            /// 返回一个成功的结果对象
            /// </summary>
            /// <param name="processId"></param>
            /// <returns></returns>
            public static StartResult SuccessResult(int? processId = null) => new()
            {
                IsSuccess = true,
                ProcessId = processId,
            };
        }
    }
}
