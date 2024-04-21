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
            await ReloadListAsync();
        }

        public async Task ReloadListAsync()
        {
            try
            {
                WebEnvironmentRepo webEnvironmentRepo = new(null);
                WebEnvironmentList = await webEnvironmentRepo.Select
                    .LeftJoin(a => a.WebBrowser != null && a.WebBrowserId == a.WebBrowser.Id)
                    .ToListAsync();

                this.StackPanel_WebEnvironmentList.Children.Clear();
                foreach (var item in WebEnvironmentList)
                {
                    WebEnvironmentListItemUserControl webEnvironmentListItemUserControl = new(item)
                    {
                        Margin = new Thickness(5)
                    };
                    this.StackPanel_WebEnvironmentList.Children.Add(webEnvironmentListItemUserControl);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
