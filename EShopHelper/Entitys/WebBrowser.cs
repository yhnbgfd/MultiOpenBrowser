namespace EShopHelper.Entitys
{
    /// <summary>
    /// 浏览器配置
    /// </summary>
    [Table(Name = nameof(WebBrowser))]
    internal class WebBrowser
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        internal TypeEnum Type { get; set; } = TypeEnum.Chrome;

        internal enum TypeEnum
        {
            Chrome,
            WebView2
        }
    }
}
