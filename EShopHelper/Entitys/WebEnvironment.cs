using System.ComponentModel;

namespace EShopHelper.Entitys
{
    /// <summary>
    /// 网络环境
    /// </summary>
    [Table(Name = nameof(WebEnvironment))]
    public class WebEnvironment : INotifyPropertyChanged
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Order { get; set; }
        public int? WebBrowserId { get; set; }
        public string? WebBrowserDataPath { get; set; }
        public string? ProxyServer { get; set; }

        [Column(IsIgnore = true)]
        public int Index { get; set; }

        public WebBrowser? WebBrowser { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static WebEnvironment Default => new()
        {
            Name = "MyWebEnvironment",
            WebBrowser = WebBrowser.Default,
            WebBrowserDataPath = GlobalData.Option.DefaultWebBrowserDataPath + "\\",
        };

        public void StartWebBrowser()
        {
            if (WebBrowser == null)
            {
                return;
            }

            WebBrowser.Start(WebBrowserDataPath, ProxyServer);
        }
    }
}
