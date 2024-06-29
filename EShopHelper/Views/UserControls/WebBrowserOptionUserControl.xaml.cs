using System.Windows;
using System.Windows.Controls;
using WebBrowser = EShopHelper.Entitys.WebBrowser;

namespace EShopHelper.Views.UserControls
{
    public partial class WebBrowserOptionUserControl : UserControl
    {
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (WebBrowser.Type)
            {
                case WebBrowser.TypeEnum.Chrome:
                    this.ComboBox_Type.SelectedIndex = 0;
                    break;
                case WebBrowser.TypeEnum.MsEdge:
                    this.ComboBox_Type.SelectedIndex = 1;
                    break;
                case WebBrowser.TypeEnum.WebView2:
                    this.ComboBox_Type.SelectedIndex = 2;
                    break;
                default:
                    this.ComboBox_Type.SelectedIndex = 0;
                    break;
            }
        }

        private void ComboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? text = ((sender as ComboBox)?.SelectedItem as ComboBoxItem)?.Content as string;
            switch (text)
            {
                case "Google Chrome":
                    WebBrowser.Type = WebBrowser.TypeEnum.Chrome;
                    break;
                case "Microsoft Edge":
                    WebBrowser.Type = WebBrowser.TypeEnum.MsEdge;
                    break;
                case "Microsoft Edge WebView2":
                    WebBrowser.Type = WebBrowser.TypeEnum.WebView2;
                    break;
                default:
                    WebBrowser.Type = WebBrowser.TypeEnum.Chrome;
                    break;
            }
        }
    }
}
