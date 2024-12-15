namespace MultiOpenBrowser.Core.WebBrowsers
{
    public interface IWebBrowser
    {
        /// <summary>
        /// 获取启动参数
        /// </summary>
        /// <param name="startOption"></param>
        /// <returns></returns>
        string? GetStartupArguments(StartOption startOption);

        /// <summary>
        /// 获取启动命令
        /// </summary>
        /// <param name="startOption"></param>
        /// <returns></returns>
        string? GetStartupCmd(StartOption startOption);

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

            public static StartOption Default => new();
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
