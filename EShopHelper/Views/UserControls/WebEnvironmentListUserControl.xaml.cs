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
                    .OrderBy(a => a.Order)
                    .OrderBy(a => a.Id)
                    .ToListAsync();

                _logger.Info($"WebEnvironmentList Count={WebEnvironmentList.Count}");

                this.StackPanel_WebEnvironmentList.Children.Clear();
                foreach (var item in WebEnvironmentList.Select((value, i) => new { i, value }))
                {
                    WebEnvironmentListItemUserControl webEnvironmentListItemUserControl = new(item.value)
                    {
                        Margin = new Thickness(5),
                    };
                    webEnvironmentListItemUserControl.DeleteClick += WebEnvironmentListItemUserControl_DeleteClick;
                    this.StackPanel_WebEnvironmentList.Children.Add(webEnvironmentListItemUserControl);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private async void WebEnvironmentListItemUserControl_DeleteClick(object sender, RoutedEventArgs e)
        {
            await ReloadListAsync();
        }
    }
}
