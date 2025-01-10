using MultiOpenBrowser.Views.Windows;
using System.IO;
using System.Windows;

namespace MultiOpenBrowser.ViewModels
{
    public class WebEnvironmentListItemViewModel : ReactiveObject
    {
        public ReactiveCommand<Unit, Unit> CopyWebEnvironmentCommand { get; }
        public WebEnvironment WebEnvironment { get; private set; }

        public WebEnvironmentListItemViewModel(WebEnvironment webEnvironment)
        {
            WebEnvironment = webEnvironment;

            CopyWebEnvironmentCommand = ReactiveCommand.Create(CopyWebEnvironment);
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
    }
}
