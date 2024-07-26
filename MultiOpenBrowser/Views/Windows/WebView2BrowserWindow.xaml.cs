using MultiOpenBrowser.Entitys;
using System.ComponentModel;
using System.Windows;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class WebView2BrowserWindow : Window, INotifyPropertyChanged
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public event PropertyChangedEventHandler? PropertyChanged;

        public WebEnvironment WebEnvironment { get; set; }
        public WebBrowser? WebBrowser => WebEnvironment.WebBrowser;

        public WebView2BrowserWindow(WebEnvironment webEnvironment)
        {
            WebEnvironment = webEnvironment;
            InitializeComponent();
            this.Title = WebEnvironment.Name;
        }
    }
}
