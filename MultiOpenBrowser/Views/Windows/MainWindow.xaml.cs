using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class MainWindow : Window
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        internal static UserInfo? UserInfo => GlobalData.UserInfo;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

#if !DEBUG
            MenuItem_DEBUG.Visibility = Visibility.Collapsed;
#endif

            SetWindowStartupLocation();

            if (GlobalData.UserInfo == null)
            {
                //this.Title = $"{this.Title} (试用版)";
            }
        }

        private void SetWindowStartupLocation()
        {
            var topCache = CacheHelper.Get("MainWindow_Top");
            var leftCache = CacheHelper.Get("MainWindow_Left");
            var widthCache = CacheHelper.Get("MainWindow_Width");
            var heightCache = CacheHelper.Get("MainWindow_Height");

            if (topCache != null && leftCache != null && widthCache != null && heightCache != null)
            {
                _ = double.TryParse(topCache, out var top);
                _ = double.TryParse(leftCache, out var left);
                _ = double.TryParse(widthCache, out var width);
                _ = double.TryParse(heightCache, out var height);

                // 获取主屏幕工作区
                var workArea = SystemParameters.WorkArea;

                // 确保窗口尺寸不超过屏幕
                width = Math.Min(width, workArea.Width);
                height = Math.Min(height, workArea.Height);

                // 确保窗口位置在屏幕内，至少留出标题栏以便可以拖动
                const double minVisibleHeight = 30;
                left = Math.Max(workArea.Left, Math.Min(left, workArea.Right - width));
                top = Math.Max(workArea.Top, Math.Min(top, workArea.Bottom - minVisibleHeight));

                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Top = top;
                this.Left = left;
                this.Width = width;
                this.Height = height;
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
            await CacheHelper.SetAsync("MainWindow_Top", this.Top);
            await CacheHelper.SetAsync("MainWindow_Left", this.Left);
            await CacheHelper.SetAsync("MainWindow_Width", this.Width);
            await CacheHelper.SetAsync("MainWindow_Height", this.Height);

            _logger.Info("MainWindow_Closed");
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CreateWebBrowserMenuItemsAsync();
            await CreateWebEnvironmentGroupMenuItemsAsync();
            EventBus.UnlockUI += UnlockUIHandle;
            EventBus.LockUI += LockUIHandle;
        }

        private async Task UnlockUIHandle()
        {
            this.DockPanel_Main.IsEnabled = true;
            await Task.CompletedTask;
        }

        private async Task LockUIHandle()
        {
            this.DockPanel_Main.IsEnabled = false;
            await Task.CompletedTask;
        }

        private async Task CreateWebBrowserMenuItemsAsync()
        {
            this.MenuItem_WebBrowser.Items.Clear();

            MenuItem menuItemAdd = new();
            menuItemAdd.SetDynamicResourceKey(TabItem.HeaderProperty, "AddBrowser");
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
                    };
                    addEnvMenuItem.SetDynamicResourceKey(TabItem.HeaderProperty, "AddEnvironment");
                    addEnvMenuItem.Click += MenuItem_AddWebEnvironmentUseWebBrowser_Click;
                    menuItem.Items.Add(addEnvMenuItem);

                    menuItem.Items.Add(new Separator());

                    MenuItem editMenuItem = new()
                    {
                        Tag = wb,
                    };
                    editMenuItem.SetDynamicResourceKey(TabItem.HeaderProperty, "Edit");
                    editMenuItem.Click += MenuItem_EditWebBrowser_Click;
                    menuItem.Items.Add(editMenuItem);

                    MenuItem deleteMenuItem = new()
                    {
                        Tag = wb,
                    };
                    deleteMenuItem.SetDynamicResourceKey(TabItem.HeaderProperty, "Delete");
                    deleteMenuItem.Click += MenuItem_DeleteWebBrowser_Click;
                    menuItem.Items.Add(deleteMenuItem);
                }
            }
        }

        private async Task CreateWebEnvironmentGroupMenuItemsAsync()
        {
            this.MenuItem_WebEnvironmentGroup.Items.Clear();
            this.MenuItem_WebEnvironment.Items.Clear();

            {
                MenuItem menuItemAdd2 = new();
                menuItemAdd2.SetDynamicResourceKey(TabItem.HeaderProperty, "AddEnvironment");
                menuItemAdd2.Click += MenuItem_AddWebEnvironment_Click;
                this.MenuItem_WebEnvironment.Items.Add(menuItemAdd2);
            }
            {
                MenuItem menuItemAdd1 = new();
                menuItemAdd1.SetDynamicResourceKey(TabItem.HeaderProperty, "AddGroup");
                menuItemAdd1.Click += MenuItem_AddWebEnvironmentGroup_Click;
                this.MenuItem_WebEnvironmentGroup.Items.Add(menuItemAdd1);
            }

            var list = await new WebEnvironmentGroupRepo(null).Select.OrderBy(a => a.Id).Take(10).ToListAsync();
            if (list.Count != 0)
            {
                this.MenuItem_WebEnvironmentGroup.Items.Add(new Separator());

                foreach (var wb in list)
                {
                    MenuItem menuItem = new()
                    {
                        Tag = wb,
                        Header = wb.Name,
                    };
                    this.MenuItem_WebEnvironmentGroup.Items.Add(menuItem);

                    MenuItem addEnvMenuItem = new()
                    {
                        Tag = wb,
                    };
                    addEnvMenuItem.SetDynamicResourceKey(TabItem.HeaderProperty, "AddEnvironment");
                    addEnvMenuItem.Click += MenuItem_AddWebEnvironmentUseWebEnvironmentGroup_Click;
                    menuItem.Items.Add(addEnvMenuItem);

                    menuItem.Items.Add(new Separator());

                    MenuItem editMenuItem = new()
                    {
                        Tag = wb,
                    };
                    editMenuItem.SetDynamicResourceKey(TabItem.HeaderProperty, "Edit");
                    editMenuItem.Click += MenuItem_EditWebEnvironmentGroup_Click;
                    menuItem.Items.Add(editMenuItem);

                    MenuItem deleteMenuItem = new()
                    {
                        Tag = wb,
                    };
                    deleteMenuItem.SetDynamicResourceKey(TabItem.HeaderProperty, "Delete");
                    deleteMenuItem.Click += MenuItem_DeleteWebEnvironmentGroup_Click;
                    menuItem.Items.Add(deleteMenuItem);
                }
            }
        }

        private void MenuItem_AddWebEnvironmentUseWebBrowser_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser tag = ((sender as MenuItem)!.Tag as WebBrowser)!;

            var newWebBrowser = (WebBrowser)tag.Clone();
            newWebBrowser.Id = 0;
            newWebBrowser.IsTemplate = false;

            new WebEnvironmentOptionWindow()
            {
                Owner = this,
                WebBrowser = newWebBrowser,
            }.ShowDialog();
        }

        private void MenuItem_AddWebEnvironmentUseWebEnvironmentGroup_Click(object sender, RoutedEventArgs e)
        {
            WebEnvironmentGroup tag = ((sender as MenuItem)!.Tag as WebEnvironmentGroup)!;

            var newWebEnvironmentGroup = (WebEnvironmentGroup)tag.Clone();

            new WebEnvironmentOptionWindow()
            {
                Owner = this,
                WebEnvironmentGroup = newWebEnvironmentGroup,
            }.ShowDialog();
        }

        private async void MenuItem_EditWebBrowser_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser webBrowser = ((sender as MenuItem)!.Tag as WebBrowser)!;

            var newWebBrowser = (WebBrowser)webBrowser.Clone();
            var dialogResult = new WebBrowserOptionWindow()
            {
                Owner = this,
                WebBrowser = newWebBrowser
            }.ShowDialog();

            if (dialogResult == true)
            {
                await CreateWebBrowserMenuItemsAsync();
            }
        }

        private async void MenuItem_EditWebEnvironmentGroup_Click(object sender, RoutedEventArgs e)
        {
            WebEnvironmentGroup tag = ((sender as MenuItem)!.Tag as WebEnvironmentGroup)!;

            var newGroup = (WebEnvironmentGroup)tag.Clone();
            var dialogResult = new WebEnvironmentGroupOptionWindow()
            {
                Owner = this,
                WebEnvironmentGroup = newGroup
            }.ShowDialog();

            if (dialogResult == true)
            {
                await CreateWebEnvironmentGroupMenuItemsAsync();
                EventBus.OnWebEnvironmentListChange?.Invoke();
            }
        }

        private async void MenuItem_DeleteWebBrowser_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser webBrowser = ((sender as MenuItem)!.Tag as WebBrowser)!;
            await new WebBrowserRepo(null).DeleteAsync(webBrowser);
            await CreateWebBrowserMenuItemsAsync();
        }

        private async void MenuItem_DeleteWebEnvironmentGroup_Click(object sender, RoutedEventArgs e)
        {
            WebEnvironmentGroup tag = ((sender as MenuItem)!.Tag as WebEnvironmentGroup)!;
            await new WebEnvironmentGroupRepo(null).DeleteAsync(tag);
            await CreateWebEnvironmentGroupMenuItemsAsync();

            EventBus.OnWebEnvironmentListChange?.Invoke();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            EventBus.LockUI -= LockUIHandle;
            EventBus.UnlockUI -= UnlockUIHandle;
        }

        private async void MenuItem_AddWebEnvironmentGroup_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = new WebEnvironmentGroupOptionWindow() { Owner = this }.ShowDialog();
            if (dialogResult == true)
            {
                await CreateWebEnvironmentGroupMenuItemsAsync();
                EventBus.OnWebEnvironmentListChange?.Invoke();
            }
        }

        private void MenuItem_ZhCN_Click(object sender, RoutedEventArgs e)
        {
            EventBus.OnLanguageChange?.Invoke("ZhCN");
        }

        private void MenuItem_EnUS_Click(object sender, RoutedEventArgs e)
        {
            EventBus.OnLanguageChange?.Invoke("EnUS");
        }

        private void MenuItem_JaJP_Click(object sender, RoutedEventArgs e)
        {
            EventBus.OnLanguageChange?.Invoke("JaJP");
        }

        private void MenuItem_KoKR_Click(object sender, RoutedEventArgs e)
        {
            EventBus.OnLanguageChange?.Invoke("KoKR");
        }

        private void MenuItem_OpenExeFolder_Click(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            Process.Start("explorer.exe", path);
        }

        private void MenuItem_OpenLogFolder_Click(object sender, RoutedEventArgs e)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            Process.Start("explorer.exe", path);
        }

        private void MenuItem_Themes_FollowSystem_Click(object sender, RoutedEventArgs e)
        {
            var systemTheme = SystemParameters.WindowGlassColor.ToString(); // 检查系统主题
            if (systemTheme == "Black") // 如果是深色模式
            {
                MenuItem_Themes_Dark_Click(sender, e);
            }
            else
            {
                MenuItem_Themes_Light_Click(sender, e);
            }
        }

        private void MenuItem_Themes_Light_Click(object sender, RoutedEventArgs e)
        {
            EventBus.OnLanguageChange?.Invoke("LightTheme");
        }

        private void MenuItem_Themes_Dark_Click(object sender, RoutedEventArgs e)
        {
            EventBus.OnLanguageChange?.Invoke("DarkTheme");
        }
    }
}