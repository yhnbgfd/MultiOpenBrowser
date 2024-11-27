namespace MultiOpenBrowser.Core.Entitys
{
    /// <summary>
    /// 网络环境
    /// </summary>
    [Table(Name = nameof(WebEnvironment))]
    public class WebEnvironment : INotifyPropertyChanged, ICloneable
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Order { get; set; }
        public int? WebEnvironmentGroupId { get; set; }
        public int? WebBrowserId { get; set; }
        public string? WebBrowserDataPath { get; set; }

        [Column(IsIgnore = true)]
        public int Index { get; set; }
        /// <summary>
        /// 记录启动后的进程ID
        /// </summary>
        [Column(IsIgnore = true)]
        public int? ProcessId { get; set; }
        [Column(IsIgnore = true)]
        public string? NameUI
        {
            get
            {
                int maxLength = 24;
                if (Name?.Length > maxLength)
                {
                    return Name.Substring(0, maxLength - 3) + "...";
                }
                return Name;
            }
        }

        public WebBrowser WebBrowser { get; set; } = WebBrowser.Default;
        public WebEnvironmentGroup? WebEnvironmentGroup { get; set; }
        public string? WebEnvironmentGroupName => WebEnvironmentGroup?.Name;
        public string? ToolTip
        {
            get
            {
                if (NameUI == Name)
                {
                    return null;
                }
                return Name;
            }
        }

        public static WebEnvironment Default => new()
        {
            Name = "MyWebEnvironment",
            WebBrowserDataPath = Path.Combine($"{GlobalData.Option.DefaultWebBrowserDataPath}", $"{DateTimeOffset.Now:yyyyMMddHHmmss}"),
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
