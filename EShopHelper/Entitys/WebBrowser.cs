using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace EShopHelper.Entitys
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
        public string? UserAgent { get; set; }
        public string? ProxyServer { get; set; }
        public bool DisableWebSecurity { get; set; } = false;
        public string? Arguments { get; set; }

        public enum TypeEnum
        {
            Chrome = 1,
            MsEdge = 2,
            WebView2 = 3,
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static WebBrowser Default => new()
        {
            Name = "MyWebBrowser",
            UserAgent = GlobalData.Option.DefaultUserAgent,
        };

        public void Start(string? userDataDir, bool incognito = false)
        {
            if (Type == TypeEnum.MsEdge)
            {
                this.StartMsEdge(userDataDir, incognito);
            }
            else if (Type == TypeEnum.WebView2)
            {
                throw new NotSupportedException();
            }
            else
            {
                this.StartChrome(userDataDir, incognito);
            }
        }

        private void StartChrome(string? userDataDir, bool incognito = false)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(userDataDir))
            {
                sb.Append($"--user-data-dir=\"{userDataDir}\" ");
            }
            sb.Append("--no-first-run ");
            sb.Append("--no-default-browser-check ");
            if (!string.IsNullOrWhiteSpace(ProxyServer))
            {
                sb.Append($"--proxy-server=\"{ProxyServer}\" ");
            }
            sb.Append("--restore-last-session ");
            sb.Append("--hide-crash-restore-bubble ");
            sb.Append("--flag-switches-begin ");
            sb.Append("--flag-switches-end ");
            if (DisableWebSecurity)
            {
                sb.Append("--disable-web-security ");//可解决跨域报错
            }
            if (!string.IsNullOrWhiteSpace(Arguments))
            {
                sb.Append($"{Arguments} ");
            }
            if (incognito == true)
            {
                sb.Append("--incognito ");
            }

            ProcessStartInfo processStartInfo = new()
            {
                FileName = GlobalData.ChromePath,
                Arguments = sb.ToString(),
            };
            Process process = new()
            {
                StartInfo = processStartInfo,
            };
            process.Start();
        }

        private void StartMsEdge(string? userDataDir, bool incognito = false)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(userDataDir))
            {
                sb.Append($"--user-data-dir=\"{userDataDir}\" ");
            }
            sb.Append("--no-first-run ");
            sb.Append("--no-default-browser-check ");
            if (!string.IsNullOrWhiteSpace(ProxyServer))
            {
                sb.Append($"--proxy-server=\"{ProxyServer}\" ");
            }
            sb.Append("--restore-last-session ");
            sb.Append("--hide-crash-restore-bubble ");
            sb.Append("--flag-switches-begin ");
            sb.Append("--flag-switches-end ");
            if (DisableWebSecurity)
            {
                sb.Append("--disable-web-security ");//可解决跨域报错
            }
            if (!string.IsNullOrWhiteSpace(Arguments))
            {
                sb.Append($"{Arguments} ");
            }
            if (incognito == true)
            {
                sb.Append("--inprivate ");
            }

            ProcessStartInfo processStartInfo = new()
            {
                FileName = GlobalData.MsEdgePath,
                Arguments = sb.ToString(),
            };
            Process process = new()
            {
                StartInfo = processStartInfo,
            };
            process.Start();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
