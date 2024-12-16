using MultiOpenBrowser.Core.WebBrowsers;
using MultiOpenBrowser.Views.Windows;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WebBrowser = MultiOpenBrowser.Core.Entitys.WebBrowser;

namespace MultiOpenBrowser.Views.UserControls
{
    public partial class WebEnvironmentListItemUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment WebEnvironment { get; private set; }
        public WebBrowser WebBrowser { get; private set; }
        public double IconOpacityDefault { get; set; } = 0.1;
        public double IconOpacityFocus { get; set; } = 0.2;

        public WebEnvironmentListItemUserControl(WebEnvironment webEnvironment)
        {
            InitializeComponent();
            DataContext = this;
            WebEnvironment = webEnvironment;
            WebBrowser = WebEnvironment.WebBrowser;
            Uri uriSource;
            if (WebBrowser.Type == WebBrowser.TypeEnum.MsEdge)
            {
                uriSource = new Uri(@"/MultiOpenBrowser;component/Assets/MicrosoftEdge.png", UriKind.Relative);
            }
            else if (WebBrowser.Type == WebBrowser.TypeEnum.Firefox)
            {
                uriSource = new Uri(@"/MultiOpenBrowser;component/Assets/Firefox.png", UriKind.Relative);
            }
            else if (WebBrowser.Type == WebBrowser.TypeEnum.Browser360)
            {
                uriSource = new Uri(@"/MultiOpenBrowser;component/Assets/360_browser.png", UriKind.Relative);
            }
            else
            {
                uriSource = new Uri(@"/MultiOpenBrowser;component/Assets/GoogleChrome.png", UriKind.Relative);
            }
            this.Image_Icon.Source = new BitmapImage(uriSource);
        }

        private void Button_StartWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new WebBrowserFactory(WebEnvironment).Start(new IWebBrowser.StartOption());
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Start WebEnvironment Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Button_DeleteWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
            if (WebEnvironment == null)
            {
                return;
            }

            var result = MessageBox.Show(Application.Current.MainWindow, $"Delete WebEnvironment: {WebEnvironment.Name} ?", "Delete WebEnvironment", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            using var uow = Global.FSql.CreateUnitOfWork();
            try
            {
                if (WebEnvironment.WebBrowser?.IsTemplate == false)
                {
                    WebBrowserRepo webBrowserRepo = new(uow);
                    await webBrowserRepo.DeleteAsync(WebEnvironment.WebBrowser);
                }

                WebEnvironmentRepo webEnvironmentRepo = new(uow);
                await webEnvironmentRepo.DeleteAsync(WebEnvironment);

                uow.Commit();

                if (!string.IsNullOrWhiteSpace(WebEnvironment.WebBrowserDataPath) && Directory.Exists(WebEnvironment.WebBrowserDataPath))
                {
                    Directory.Delete(WebEnvironment.WebBrowserDataPath, true);
                }
            }
            catch (Exception ex)
            {
                uow.Rollback();
                _logger.Error(ex);
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Delete WebEnvironment Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                EventBus.OnWebEnvironmentListChange?.Invoke();
            }
        }

        private void Button_EditWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
            if (WebEnvironment == null)
            {
                return;
            }

            var newWebEnvironment = (WebEnvironment)WebEnvironment.Clone();

            _ = new WebEnvironmentOptionWindow()
            {
                Owner = Application.Current.MainWindow,
                WebEnvironment = newWebEnvironment
            }.ShowDialog();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Image_Icon.Opacity = IconOpacityFocus;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Image_Icon.Opacity = IconOpacityDefault;
        }

        private void Button_StartWebEnvironmentIncognito_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new WebBrowserFactory(WebEnvironment).Start(new IWebBrowser.StartOption() { IncognitoMode = true });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Start WebEnvironment Incognito Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (WebEnvironment == null)
            {
                return;
            }

            try
            {
                EventBus.LockUI?.Invoke();

                var newWebEnvironment = (WebEnvironment)WebEnvironment.Clone();

                var sourceDataPath = newWebEnvironment.WebBrowserDataPath;

                newWebEnvironment.Id = 0;
                newWebEnvironment.Order = 0;
                newWebEnvironment.WebBrowser.Id = 0;
                newWebEnvironment.Name += " Copy";
                newWebEnvironment.WebBrowserDataPath = Path.Combine($"{GlobalData.Option.DefaultWebBrowserDataPath}", $"{DateTimeOffset.Now:yyyyMMddHHmmss}");

                var dialogResult = new WebEnvironmentOptionWindow()
                {
                    Owner = Application.Current.MainWindow,
                    WebEnvironment = newWebEnvironment
                }.ShowDialog();

                if (dialogResult == true && sourceDataPath != null && Directory.Exists(sourceDataPath))
                {
                    FileHelper.CopyDirectory(sourceDataPath, newWebEnvironment.WebBrowserDataPath, true);
                }
            }
            finally
            {
                EventBus.UnlockUI?.Invoke();
            }
        }

        private void MenuItem_OpenDataFolder_Click(object sender, RoutedEventArgs e)
        {
            if (WebEnvironment == null)
            {
                return;
            }

            if (WebEnvironment.WebBrowserDataPath != null && Directory.Exists(WebEnvironment.WebBrowserDataPath))
            {
                Process.Start("explorer.exe", WebEnvironment.WebBrowserDataPath);
            }
        }

        private void MenuItem_CopyStartupCMD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WebEnvironment == null)
                {
                    return;
                }

                var cmd = new WebBrowserFactory(WebEnvironment).GetStartupCmd(new IWebBrowser.StartOption());

                Clipboard.SetText(cmd, TextDataFormat.Text);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Copy Startup CMD Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
