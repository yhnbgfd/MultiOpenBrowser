using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace EShopHelper.Entitys
{
    /// <summary>
    /// 浏览器配置
    /// </summary>
    [Table(Name = nameof(WebBrowser))]
    [Obfuscation(Exclude = true)]
    public class WebBrowser : INotifyPropertyChanged
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Order { get; set; }
        public bool IsTemplate { get; set; } = false;
        public TypeEnum Type { get; set; } = TypeEnum.Chrome;
        public string? UserAgent { get; set; }

        public enum TypeEnum
        {
            Chrome,
            WebView2,
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static WebBrowser Default => new()
        {
            Name = "MyWebBrowser",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36 NGA_WP_JW/(;WINDOWS)",
        };

        public void Start(string? userDataDir, string? proxyServer)
        {
            if (Type == TypeEnum.Chrome)
            {
                StartChrome(userDataDir, proxyServer);
            }
        }

        private static void StartChrome(string? userDataDir, string? proxyServer)
        {
            var chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(userDataDir))
            {
                sb.Append($"--user-data-dir=\"{userDataDir}\" ");
            }
            sb.Append("--no-first-run ");
            sb.Append("--no-default-browser-check ");
            if (!string.IsNullOrWhiteSpace(proxyServer))
            {
                sb.Append($"--proxy-server=\"{proxyServer}\" ");
            }
            sb.Append("--restore-last-session ");
            sb.Append("--hide-crash-restore-bubble ");
            sb.Append("--flag-switches-begin ");
            sb.Append("--flag-switches-end ");

            ProcessStartInfo processStartInfo = new()
            {
                FileName = chromePath,
                Arguments = sb.ToString(),
            };
            Process process = new()
            {
                StartInfo = processStartInfo,
            };
            process.Start();
        }
    }
}
