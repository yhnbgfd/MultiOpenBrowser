namespace MultiOpenBrowser.Core.Entitys
{
    /// <summary>
    /// 浏览器配置
    /// </summary>
    [Table(Name = nameof(WebBrowser))]
    public class WebBrowser : INotifyPropertyChanged, ICloneable
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Order { get; set; }
        public bool IsTemplate { get; set; } = false;
        public TypeEnum Type { get; set; } = TypeEnum.Chrome;
        /// <summary>
        /// 自定义浏览器程序exe文件目录
        /// </summary>
        public string? ExePath { get; set; }
        public string? UserAgent { get; set; }
        public string? ProxyServer { get; set; }
        public bool DisableWebSecurity { get; set; } = false;
        public bool RestoreLastSession { get; set; } = true;
        public string? Arguments { get; set; }
        /// <summary>
        /// 默认网站
        /// </summary>
        public string? DefaultWebsite { get; set; }
        /// <summary>
        /// 默认网站是否以APP模式打开
        /// </summary>
        public bool IsApp { get; set; } = false;

        [Column(IsIgnore = true)]
        public static WebBrowser Default => new()
        {
            Name = "MyWebBrowser",
            UserAgent = GlobalData.Option.DefaultUserAgent,
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public enum TypeEnum
        {
            [Description("Google Chrome")]
            Chrome = 1,
            [Description("Microsoft Edge")]
            MsEdge = 2,
            [Description("Firefox")]
            Firefox = 4,
            [Description("Browser360")]
            Browser360 = 5,
            [Description("Other")]
            Other = 99,
        }
    }
}
