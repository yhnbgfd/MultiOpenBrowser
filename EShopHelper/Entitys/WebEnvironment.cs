using System.ComponentModel;
using System.IO;

namespace EShopHelper.Entitys
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
        public int? WebBrowserId { get; set; }
        public string? WebBrowserDataPath { get; set; }

        [Column(IsIgnore = true)]
        public int Index { get; set; }

        public WebBrowser WebBrowser { get; set; } = WebBrowser.Default;

        public event PropertyChangedEventHandler? PropertyChanged;

        public static WebEnvironment Default => new()
        {
            Name = "MyWebEnvironment",
            WebBrowserDataPath = Path.Combine($"{GlobalData.Option.DefaultWebBrowserDataPath}", $"{DateTimeOffset.Now:yyyyMMddHHmmss}"),
        };

        public void StartWebBrowser(bool incognito = false)
        {
            if (WebBrowser == null)
            {
                return;
            }

            WebBrowser.Start(WebBrowserDataPath, incognito);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
