using System.Windows;

namespace EShopHelper
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //StartupUri = new Uri("Views/Windows/MainWindow.xaml", UriKind.Relative);
            StartupUri = new Uri("Views/Windows/TestWindow.xaml", UriKind.Relative);
        }
    }

}
