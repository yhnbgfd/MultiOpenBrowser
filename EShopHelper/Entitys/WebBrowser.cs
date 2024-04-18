using System.Reflection;

namespace EShopHelper.Entitys
{
    /// <summary>
    /// 浏览器配置
    /// </summary>
    [Table(Name = nameof(WebBrowser))]
    [Obfuscation(Exclude = true)]
    public class WebBrowser
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TypeEnum Type { get; set; } = TypeEnum.Chrome;
        public string UserAgent { get; set; } = string.Empty;

        public enum TypeEnum
        {
            Chrome,
            WebView2
        }
    }
}
