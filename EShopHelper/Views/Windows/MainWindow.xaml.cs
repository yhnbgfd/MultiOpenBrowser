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

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Options_Click(object sender, RoutedEventArgs e)
        {
            new OptionsWindow() { Owner = this }.ShowDialog();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow() { Owner = this }.ShowDialog();
        }

        private void MenuItem_CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("当前为最新版");
        }
    }
}