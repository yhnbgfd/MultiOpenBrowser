using NLog;
using System.Windows;

namespace EShopHelper.Views.Windows
{
    public partial class WebEnvironmentOptionWindow : Window
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment WebEnvironment { get; set; } = WebEnvironment.Default;
        public WebBrowser? WebBrowser => WebEnvironment.WebBrowser;

        public WebEnvironmentOptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            using var uow = Global.FSql.CreateUnitOfWork();
            try
            {
                WebBrowserRepo webBrowserRepo = new(uow);
                WebEnvironment.WebBrowser = await webBrowserRepo.InsertOrUpdateAsync(WebEnvironment.WebBrowser!);

                WebEnvironment.WebBrowserId = WebEnvironment.WebBrowser.Id;

                WebEnvironmentRepo webEnvironmentRepo = new(uow);
                await webEnvironmentRepo.InsertOrUpdateAsync(WebEnvironment);

                uow.Commit();

                this.Close();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                _logger.Error(ex);
            }
        }
    }
}
