using MultiOpenBrowser.Core.WebBrowsers;
using MultiOpenBrowser.Views.Windows;
using ReactiveUI;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MultiOpenBrowser.Views.UserControls
{
    public partial class WebEnvironmentListItemUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const double IconOpacityDefault = 0.1;
        private const double IconOpacityFocus = 0.2;

        public WebEnvironment WebEnvironment { get; private set; }
        public ReactiveCommand<Unit, Unit> CopyWebEnvironmentCommand { get; }

        public WebEnvironmentListItemUserControl(WebEnvironment webEnvironment)
        {
            InitializeComponent();
            DataContext = this;
            WebEnvironment = webEnvironment;

            CopyWebEnvironmentCommand = ReactiveCommand.Create(() =>
            {
                try
                {
                    EventBus.LockUI?.Invoke();
                    CopyWebEnvironment();
                }
                finally
                {
                    EventBus.UnlockUI?.Invoke();
                }
            });

            WebBrowserFactory webBrowserFactory = new(WebEnvironment);
            var iconName = webBrowserFactory.WebBrowserInstance.Icon;
            var iconPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", iconName);
            Uri uriSource = new(iconPath);
            this.Image_Icon.Source = new BitmapImage(uriSource);
            this.Image_Icon.Opacity = IconOpacityDefault;
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

        private void CopyWebEnvironment()
        {
            if (WebEnvironment == null)
            {
                return;
            }

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
    }
}
