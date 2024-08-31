namespace MultiOpenBrowser.Entitys
{
    public class Option : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 默认浏览器数据文件夹
        /// </summary>
        public string DefaultWebBrowserDataPath { get; set; } = string.Empty;
        /// <summary>
        /// 默认 User-Agent
        /// </summary>
        public string DefaultUserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36 NGA_WP_JW/(;WINDOWS)";
    }
}
