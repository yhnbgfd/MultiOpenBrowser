﻿using System.Windows;
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
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            _logger.Info($"Application_Startup {string.Join(" ", e.Args)}");

#if DEBUG
            //await ArgsHelper.StartWebEnvironmentAsync("--start-web-environment=21");
#endif
            await ArgsHelper.StartWebEnvironmentAsync(e.Args);

            StartupUri = new Uri("Views/Windows/MainWindow.xaml", UriKind.Relative);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "DispatcherUnhandledException");
            Environment.Exit(1);
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "UnhandledException");
            Environment.Exit(1);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show((e.ExceptionObject as Exception)?.ToString(), "UnobservedTaskException");
            Environment.Exit(1);
        }
    }
}