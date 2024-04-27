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

            var topCache = CacheRepo.Get("MainWindow_Top");
            var leftCache = CacheRepo.Get("MainWindow_Left");
            if (topCache != null && leftCache != null)
            {
                _ = double.TryParse(topCache.Value, out var top);
                _ = double.TryParse(leftCache.Value, out var left);
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Top = top;
                this.Left = left;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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
            MessageBox.Show(this, "当前为最新版");
        }

        private async void Window_Closed(object sender, EventArgs e)
        {
            await CacheRepo.SetAsync("MainWindow_Top", this.Top, null);
            await CacheRepo.SetAsync("MainWindow_Left", this.Left, null);
        }
    }
}