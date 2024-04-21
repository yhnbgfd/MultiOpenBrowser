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
    public class WebBrowser
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsTemplate { get; set; } = false;
        public TypeEnum Type { get; set; } = TypeEnum.Chrome;
        public string UserAgent { get; set; } = string.Empty;

        public enum TypeEnum
        {
            Chrome,
            WebView2,
        }

        public void Start(string userDataDir, string? proxyServer = null)
        {
            if (Type == TypeEnum.Chrome)
            {
                StartChrome(userDataDir, proxyServer);
            }
        }

        private static void StartChrome(string userDataDir, string? proxyServer = null)
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
