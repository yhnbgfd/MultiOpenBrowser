using System.Windows;
using System.Windows.Threading;

namespace MultiOpenBrowser
{
    public partial class App : Application
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public App()
        {
            // 在异常由应用程序引发但未进行处理时发生。UI线程
            // 无法捕获多线程异常
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            // 专门捕获所有线程中的异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            // 专门捕获Task异常
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            EventBus.OnLanguageChange += OnLanguageChangeHandle;
        }

        private void OnLanguageChangeHandle(string langName)
        {
            if (Application.LoadComponent(new Uri(@"Views\Resources\" + langName + ".xaml", UriKind.Relative)) is ResourceDictionary langRd)
            {
                Resources.MergedDictionaries[0] = langRd;
            }
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            _logger.Info($"Application_Startup {string.Join(" ", e.Args)}");

            await ArgsHelper.StartWebEnvironmentAsync(e.Args);

            StartupUri = new Uri("Views/Windows/MainWindow.xaml", UriKind.Relative);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "DispatcherUnhandledException", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "UnhandledException", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show((e.ExceptionObject as Exception)?.ToString(), "UnobservedTaskException", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
