using System.Windows;

namespace EShopHelper.Views.Windows
{
    public partial class WebBrowserOptionWindow : Window
    {
        internal WebBrowser WebBrowser { get; set; } = new();

        public WebBrowserOptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser = new()
            {
                Name = "浏览器1",
                Type = WebBrowser.TypeEnum.Chrome,
            };

            WebBrowserRepo webBrowserRepo = new(null);
            await webBrowserRepo.InsertOrUpdateAsync(WebBrowser);

            this.Close();
        }
    }
}
