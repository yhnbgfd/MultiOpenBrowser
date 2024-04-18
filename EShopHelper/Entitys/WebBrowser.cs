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
        public bool IsTemplate { get; set; } = false;
        public TypeEnum Type { get; set; } = TypeEnum.Chrome;
        public SystemEnum System { get; set; } = SystemEnum.Window11;
        public string UserAgent { get; set; } = string.Empty;

        public enum TypeEnum
        {
            Chrome,
            WebView2,
        }

        public enum SystemEnum
        {
            Window11,
            Window10,
            Window7,
        }

        public void Start()
        {
            if (Type == TypeEnum.Chrome)
            {

            }
        }
    }
}
