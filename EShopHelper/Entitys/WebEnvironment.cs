using System.Reflection;

namespace EShopHelper.Entitys
{
    /// <summary>
    /// 网络环境
    /// </summary>
    [Table(Name = nameof(WebEnvironment))]
    [Obfuscation(Exclude = true)]
    public class WebEnvironment
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? WebBrowserId { get; set; }
        public string WebBrowserDataPath { get; set; } = string.Empty;

        public WebBrowser? WebBrowser { get; set; }

        public void StartWebBrowser()
        {
            if (WebBrowser == null)
            {
                return;
            }

            WebBrowser.Start(WebBrowserDataPath);
        }
    }
}
