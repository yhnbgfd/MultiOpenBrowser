using System.Windows;

namespace EShopHelper.Views.Windows
{
    public partial class WebBrowserOptionWindow : Window
    {
        public WebBrowserOptionWindow()
        {
            InitializeComponent();
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser webBrowser = new()
            {
                Name = "浏览器1",
                Type = WebBrowser.TypeEnum.Chrome,
            };

            WebBrowserRepo webBrowserRepo = new(null);
            await webBrowserRepo.InsertAsync(webBrowser);

            this.Close();
        }
    }
}
