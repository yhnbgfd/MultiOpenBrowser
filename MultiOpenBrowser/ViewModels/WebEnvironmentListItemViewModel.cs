using MultiOpenBrowser.Core.WebBrowsers;
using MultiOpenBrowser.Views.Windows;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MultiOpenBrowser.ViewModels
{
    public class WebEnvironmentListItemViewModel : ReactiveObject
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment WebEnvironment { get; private set; }
        public ReactiveCommand<Unit, Unit> StartWebEnvironmentCommand { get; }
        public ReactiveCommand<Unit, Unit> StartWebEnvironmentIncognitoCommand { get; }
        public ReactiveCommand<Unit, Unit> EditWebEnvironmentCommand { get; }
        public ReactiveCommand<Unit, Unit> CopyWebEnvironmentCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenDataFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteWebEnvironmentCommand { get; }
        public ReactiveCommand<Unit, Unit> CopyStartupCMDCommand { get; }

        public WebEnvironmentListItemViewModel(WebEnvironment webEnvironment)
        {
            WebEnvironment = webEnvironment;

            StartWebEnvironmentCommand = ReactiveCommand.Create(StartWebEnvironment);
            StartWebEnvironmentIncognitoCommand = ReactiveCommand.Create(StartWebEnvironmentIncognito);
            EditWebEnvironmentCommand = ReactiveCommand.Create(EditWebEnvironment);
            CopyWebEnvironmentCommand = ReactiveCommand.Create(CopyWebEnvironment);
            OpenDataFolderCommand = ReactiveCommand.Create(OpenDataFolder);
            DeleteWebEnvironmentCommand = ReactiveCommand.CreateFromTask(DeleteWebEnvironmentAsync);
            CopyStartupCMDCommand = ReactiveCommand.Create(CopyStartupCMD);
        }

        public void StartWebEnvironment()
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

        private void StartWebEnvironmentIncognito()
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

        private void EditWebEnvironment()
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

        private void CopyWebEnvironment()
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

        private void OpenDataFolder()
        {
            if (WebEnvironment == null)
            {
                return;
            }

            if (WebEnvironment.WebBrowserDataPath != null && Directory.Exists(WebEnvironment.WebBrowserDataPath))
            {
                Process.Start("explorer.exe", WebEnvironment.WebBrowserDataPath);
            }
            else
            {

            }
        }

        private async Task DeleteWebEnvironmentAsync()
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

        private void CopyStartupCMD()
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
