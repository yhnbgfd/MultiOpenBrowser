using NLog;
using System.Windows;
using System.Windows.Controls;
using WebBrowser = EShopHelper.Entitys.WebBrowser;

namespace EShopHelper.Views.UserControls
{
    public partial class WebBrowserOptionUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static readonly DependencyProperty WebBrowserProperty = DependencyProperty.Register(
            "WebBrowser", typeof(WebBrowser), typeof(WebBrowserOptionUserControl), new PropertyMetadata(new WebBrowser() { Name = "WebBrowser" }));

        public WebBrowser WebBrowser
        {
            get { return (WebBrowser)GetValue(WebBrowserProperty); }
            set { SetValue(WebBrowserProperty, value); }
        }

        public WebBrowserOptionUserControl()
        {
            InitializeComponent();
        }
    }
}
