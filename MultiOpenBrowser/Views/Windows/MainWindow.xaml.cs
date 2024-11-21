using System.Windows;
using System.Windows.Controls;
using WebBrowser = MultiOpenBrowser.Entitys.WebBrowser;

namespace MultiOpenBrowser.Views.Windows
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

            if (GlobalData.UserInfo == null)
            {
                //this.Title = $"{this.Title} (试用版)";
            }
        }

        private async void MenuItem_AddWebBrowser_Click(object sender, RoutedEventArgs e)
        {
            new WebBrowserOptionWindow() { Owner = this }.ShowDialog();
            await CreateWebBrowserMenuItemsAsync();
        }

        private void MenuItem_AddWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
            new WebEnvironmentOptionWindow() { Owner = this }.ShowDialog();
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

        private async void Window_Closed(object sender, EventArgs e)
        {
            await CacheRepo.SetAsync("MainWindow_Top", this.Top, null);
            await CacheRepo.SetAsync("MainWindow_Left", this.Left, null);
            await CacheRepo.SetAsync("MainWindow_Width", this.Width, null);
            await CacheRepo.SetAsync("MainWindow_Height", this.Height, null);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CreateWebBrowserMenuItemsAsync();
            EventBus.UnlockUI += UnlockUIHandle;
            EventBus.LockUI += LockUIHandle;
        }

        private async Task UnlockUIHandle()
        {
            this.DockPanel_Main.IsEnabled = true;
        }

        private async Task LockUIHandle()
        {
            this.DockPanel_Main.IsEnabled = false;
        }

        private async Task CreateWebBrowserMenuItemsAsync()
        {
            this.MenuItem_WebBrowser.Items.Clear();

            MenuItem menuItemAdd = new()
            {
                Header = "Add Browser",
            };
            menuItemAdd.Click += MenuItem_AddWebBrowser_Click;
            this.MenuItem_WebBrowser.Items.Add(menuItemAdd);

            var webBrowserList = await new WebBrowserRepo(null).Select.Where(a => a.IsTemplate == true).OrderBy(a => a.Id).Take(10).ToListAsync();
            if (webBrowserList.Count != 0)
            {
                this.MenuItem_WebBrowser.Items.Add(new Separator());

                foreach (var wb in webBrowserList)
                {
                    MenuItem menuItem = new()
                    {
                        Tag = wb,
                        Header = wb.Name,
                    };
                    this.MenuItem_WebBrowser.Items.Add(menuItem);

                    MenuItem addEnvMenuItem = new()
                    {
                        Tag = wb,
                        Header = "Add Environment",
                    };
                    addEnvMenuItem.Click += MenuItem_WebBrowser_AddByWebEnvironment_Click;
                    menuItem.Items.Add(addEnvMenuItem);

                    menuItem.Items.Add(new Separator());

                    MenuItem editMenuItem = new()
                    {
                        Tag = wb,
                        Header = "Edit",
                    };
                    editMenuItem.Click += MenuItem_WebBrowser_EditWebBrowser_Click; ;
                    menuItem.Items.Add(editMenuItem);

                    MenuItem deleteMenuItem = new()
                    {
                        Tag = wb,
                        Header = "Delete",
                    };
                    deleteMenuItem.Click += MenuItem_WebBrowser_DeleteWebBrowser_Click; ;
                    menuItem.Items.Add(deleteMenuItem);
                }
            }
        }

        private void MenuItem_WebBrowser_AddByWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser webBrowser = ((sender as MenuItem)!.Tag as WebBrowser)!;

            var newWebBrowser = (WebBrowser)webBrowser.Clone();
            newWebBrowser.Id = 0;
            newWebBrowser.IsTemplate = false;

            new WebEnvironmentOptionWindow()
            {
                Owner = this,
                WebBrowser = newWebBrowser,
            }.ShowDialog();
        }

        private async void MenuItem_WebBrowser_EditWebBrowser_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser webBrowser = ((sender as MenuItem)!.Tag as WebBrowser)!;

            var newWebBrowser = (WebBrowser)webBrowser.Clone();
            new WebBrowserOptionWindow()
            {
                Owner = this,
                WebBrowser = newWebBrowser
            }.ShowDialog();
            await CreateWebBrowserMenuItemsAsync();
        }

        private async void MenuItem_WebBrowser_DeleteWebBrowser_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser webBrowser = ((sender as MenuItem)!.Tag as WebBrowser)!;
            await new WebBrowserRepo(null).DeleteAsync(webBrowser);
            await CreateWebBrowserMenuItemsAsync();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            EventBus.LockUI -= LockUIHandle;
            EventBus.UnlockUI -= UnlockUIHandle;
        }

        private void MenuItem_AddWebEnvironmentGroup_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}