using System.Windows;
using System.Windows.Controls;
using WebBrowser = MultiOpenBrowser.Core.Entitys.WebBrowser;

namespace MultiOpenBrowser.Views.UserControls
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
            this.ComboBox_Type.SelectedIndex = WebBrowser.Type switch
            {
                WebBrowser.TypeEnum.Chrome => 0,
                WebBrowser.TypeEnum.MsEdge => 1,
                WebBrowser.TypeEnum.WebView2 => 2,
                WebBrowser.TypeEnum.Firefox => 3,
                WebBrowser.TypeEnum.Other => 4,
                _ => 0,
            };
        }

        private void ComboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? text = ((sender as ComboBox)?.SelectedItem as ComboBoxItem)?.Content as string;
            WebBrowser.Type = text switch
            {
                "Google Chrome" => WebBrowser.TypeEnum.Chrome,
                "Microsoft Edge" => WebBrowser.TypeEnum.MsEdge,
                "Microsoft Edge WebView2" => WebBrowser.TypeEnum.WebView2,
                "Firefox" => WebBrowser.TypeEnum.Firefox,
                "Other" => WebBrowser.TypeEnum.Other,
                _ => WebBrowser.TypeEnum.Chrome,
            };
        }
    }
}
