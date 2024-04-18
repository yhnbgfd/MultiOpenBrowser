using NLog;
using System.Windows;
using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        internal List<WebEnvironment> WebEnvironmentList { get; set; } = [];

        public WebEnvironmentListUserControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _logger.Debug("Loaded");

            await ReloadListAsync();

            _logger.Debug("Load Data OK");
        }

        public async Task ReloadListAsync()
        {
            WebEnvironmentRepo webEnvironmentRepo = new(null);
            WebEnvironmentList = await webEnvironmentRepo.Select.ToListAsync();
        }
    }
}
