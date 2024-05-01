using System.Windows;

namespace EShopHelper.Views.Windows
{
    public partial class MainWindow : Window
    {
        internal static UserInfo? UserInfo => GlobalData.UserInfo;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var topCache = CacheRepo.Get("MainWindow_Top");
            var leftCache = CacheRepo.Get("MainWindow_Left");
            var widthCache = CacheRepo.Get("MainWindow_Width");
            var heightCache = CacheRepo.Get("MainWindow_Height");
            if (topCache != null && leftCache != null && widthCache != null && heightCache != null)
            {
                _ = double.TryParse(topCache, out var top);
                _ = double.TryParse(leftCache, out var left);
                _ = double.TryParse(widthCache, out var width);
                _ = double.TryParse(heightCache, out var height);
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Top = top;
                this.Left = left;
                this.Width = width;
                this.Height = height;
            }
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
            await CacheRepo.SetAsync("MainWindow_Width", this.Width, null);
            await CacheRepo.SetAsync("MainWindow_Height", this.Height, null);
        }
    }
}