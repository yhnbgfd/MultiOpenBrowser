using System.Windows;

namespace EShopHelper.Views.Windows
{
    public partial class MainWindow : Window
    {
        internal UserInfo? UserInfo => GlobalData.UserInfo;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void MenuItem_AddWebBrowser_Click(object sender, RoutedEventArgs e)
        {
            new WebBrowserOptionWindow() { Owner = this }.ShowDialog();
        }

        private async void MenuItem_AddWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
            new WebEnvironmentOptionWindow() { Owner = this }.ShowDialog();
            await WebEnvironmentListUC.ReloadListAsync();
        }
    }
}