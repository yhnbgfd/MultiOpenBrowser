using NLog;
using System.Windows;

namespace EShopHelper
{
    public partial class App : Application
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            _logger.Info($"Application_Startup {string.Join(" ", e.Args)}");

            await StartWebEnvironmentAsync(e.Args);

            StartupUri = new Uri("Views/Windows/MainWindow.xaml", UriKind.Relative);
        }

        /// <summary>
        /// 启动WebEnvironment
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static async Task StartWebEnvironmentAsync(string[] args)
        {
            var startWebEnvironmenArgs = args.FirstOrDefault(a => a.StartsWith("--start-web-environment="));
            if (startWebEnvironmenArgs != null)
            {
                var startWebEnvironmenArgsSplit = startWebEnvironmenArgs.Split("=", StringSplitOptions.RemoveEmptyEntries);
                if (startWebEnvironmenArgsSplit.Length > 1)
                {
                    _ = int.TryParse(startWebEnvironmenArgsSplit[1], out var id);

                    WebEnvironmentRepo webEnvironmentRepo = new(null);
                    var webEnvironmen = await webEnvironmentRepo.Select
                        .Where(a => a.Id == id)
                        .LeftJoin(a => a.WebBrowser != null && a.WebBrowserId == a.WebBrowser.Id)
                        .FirstAsync();
                    webEnvironmen?.StartWebBrowser();
                    Environment.Exit(0);
                }
            }
        }
    }
}
